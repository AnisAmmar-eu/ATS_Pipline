using System.Buffers.Binary;
using Core.Shared.Models.DB.Kernel.Interfaces;
using Core.Shared.Models.DTO.Kernel.Interfaces;
using Core.Shared.Services.Kernel.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using TwinCAT.Ads;

namespace ApiADS.Notifications;

public class
	BaseNotification<TService, T, TDTO, TStruct>
	where T : class, IBaseEntity<T, TDTO>
	where TDTO : class, IDTO<T, TDTO>
	where TService : class, IServiceBaseEntity<T, TDTO>
	where TStruct : struct, IBaseADS<T, TStruct>
{
	private readonly uint _acquitMsg;
	private readonly uint _newMsg;
	private readonly uint _oldEntry;
	private readonly ResultHandle _resultHandle;

	public BaseNotification(ResultHandle resultHandle, uint acquitMsg, uint newMsg, uint oldEntry)
	{
		_resultHandle = resultHandle;
		_acquitMsg = acquitMsg;
		_newMsg = newMsg;
		_oldEntry = oldEntry;
	}

	protected static async Task<BaseNotification<TService, T, TDTO, TStruct>> CreateSub(dynamic ads,
		string acquitSymbol, string newMsgSymbol, string oldEntrySymbol)
	{
		AdsClient tcClient = (AdsClient)ads.tcClient;

		uint acquitMsg = tcClient.CreateVariableHandle(acquitSymbol);
		uint newMsg = tcClient.CreateVariableHandle(newMsgSymbol);
		uint oldEntry = tcClient.CreateVariableHandle(oldEntrySymbol);

		int size = sizeof(uint);
		ResultHandle resultHandle = await tcClient.AddDeviceNotificationAsync(newMsgSymbol, size,
			new NotificationSettings(AdsTransMode.OnChange, 0, 0), ads, ads.cancel);
		BaseNotification<TService, T, TDTO, TStruct> notification =
			new(resultHandle, acquitMsg, newMsg, oldEntry);
		tcClient.AdsNotification += notification.GetElement;

		// Verifies if there isn't already something in the queue
		ResultValue<uint> newMsgValue = await tcClient.ReadAnyAsync<uint>(ads.msgNewHandle, ads.cancel);
		if (newMsgValue.ErrorCode != AdsErrorCode.NoError)
			throw new Exception(newMsgValue.ErrorCode.ToString());
		if (newMsgValue.Value == Utils.HasNewMsg)
			notification.GetElementSub(ads);
		return notification;
	}

	public void GetElement(object? sender, AdsNotificationEventArgs e)
	{
		uint newMsg = BinaryPrimitives.ReadUInt32LittleEndian(e.Data.Span);
		if (e.Handle == _resultHandle.Handle && newMsg == Utils.HasNewMsg)
		{
			Console.WriteLine("Notif msgNew");
			// UserData is our data passed in parameter
			GetElementSub(e.UserData as dynamic);
		}
	}

	public async void GetElementSub(dynamic dynamicObject)
	{
		AdsClient? tcClient = dynamicObject.tcClient as AdsClient;
		tcClient!.WriteAny(_acquitMsg, Utils.IsReading);
		tcClient.WriteAny(_newMsg, Utils.NoMessage);
		// Get element of FIFO
		TStruct adsStruct = (TStruct)tcClient.ReadAny(_oldEntry, typeof(TStruct));
		T entity = adsStruct.ToModel();
		await using AsyncServiceScope scope = ((IServiceProvider)dynamicObject.appServices).CreateAsyncScope();
		IServiceProvider services = scope.ServiceProvider;

		try
		{
			await AddElement(services, entity);
			tcClient.WriteAny(_acquitMsg, Utils.FinishedReading);
		}
		catch
		{
			// Retry
			tcClient.WriteAny(_acquitMsg, Utils.ErrorWhileReading);
		}
	}
	
	protected virtual async Task AddElement(IServiceProvider services, T entity)
	{
			TService serviceAds = services.GetRequiredService<TService>();
			await serviceAds.Add(entity);
	}
}