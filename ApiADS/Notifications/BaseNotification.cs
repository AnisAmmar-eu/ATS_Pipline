using Core.Shared.Models.DB.Kernel.Interfaces;
using Core.Shared.Models.DTO.Kernel.Interfaces;
using Core.Shared.Services.Kernel.Interfaces;
using TwinCAT.Ads;

namespace ApiADS.Notifications;

public class
	BaseNotification<TService, T, TDTO, TStruct>
	where T : class, IBaseEntity<T, TDTO>
	where TDTO : class, IDTO<T, TDTO>
	where TService : class, IServiceBaseEntity<T, TDTO>
	where TStruct : struct, IBaseADS<T, TStruct>
{
	private uint _remove;
	private uint _newMsg;
	private uint _oldEntry;
	private ResultHandle _resultHandle = null!;

	public BaseNotification(ResultHandle resultHandle, uint remove, uint newMsg, uint oldEntry)
	{
		_resultHandle = resultHandle;
		_remove = remove;
		_newMsg = newMsg;
		_oldEntry = oldEntry;
	}

	protected BaseNotification()
	{
	}

	protected static async Task<BaseNotification<TService, T, TDTO, TStruct>> CreateSub<TNotification>(dynamic ads,
		string removeSymbol, string newMsgSymbol, string oldEntrySymbol)
		where TNotification : BaseNotification<TService, T, TDTO, TStruct>, new()
	{
		AdsClient tcClient = (AdsClient)ads.tcClient;

		uint remove = tcClient.CreateVariableHandle(removeSymbol);
		uint newMsg = tcClient.CreateVariableHandle(newMsgSymbol);
		uint oldEntry = tcClient.CreateVariableHandle(oldEntrySymbol);

		int size = sizeof(bool);
		ResultHandle resultHandle = await tcClient.AddDeviceNotificationAsync(newMsgSymbol, size,
			new NotificationSettings(AdsTransMode.OnChange, 0, 0), ads, ads.cancel);
		TNotification notification =
			new()
			{
				_remove = remove,
				_newMsg = newMsg,
				_oldEntry = oldEntry,
				_resultHandle = resultHandle
			};
		tcClient.AdsNotification += notification.GetElement;

		// Verifies if there isn't already something in the queue
		ResultValue<bool> newMsgValue = await tcClient.ReadAnyAsync<bool>(newMsg, ads.cancel);
		if (newMsgValue.ErrorCode != AdsErrorCode.NoError)
			throw new Exception(newMsgValue.ErrorCode.ToString());
		if (newMsgValue.Value)
			notification.GetElementSub(ads);
		return notification;
	}

	public void GetElement(object? sender, AdsNotificationEventArgs e)
	{
		bool newMsg = BitConverter.ToBoolean(e.Data.Span);
		if (e.Handle == _resultHandle.Handle && newMsg)
		{
			Console.WriteLine("Notif msgNew");
			// UserData is our data passed in parameter
			GetElementSub(e.UserData as dynamic);
		}
	}

	public async void GetElementSub(dynamic dynamicObject)
	{
		AdsClient? tcClient = dynamicObject.tcClient as AdsClient;
		//tcClient!.WriteAny(_acquitMsg, Utils.IsReading);
		tcClient!.WriteAny(_newMsg, false);
		// Get element of FIFO
		TStruct adsStruct = tcClient.ReadAny<TStruct>(_oldEntry);
		T entity = adsStruct.ToModel();
		await using AsyncServiceScope scope = ((IServiceProvider)dynamicObject.appServices).CreateAsyncScope();
		IServiceProvider services = scope.ServiceProvider;

		try
		{
			await AddElement(services, entity);
			tcClient.WriteAny(_remove, true);
		}
		catch
		{
			// Retry
			//tcClient.WriteAny(_acquitMsg, Utils.ErrorWhileReading);
		}
	}

	protected virtual async Task AddElement(IServiceProvider services, T entity)
	{
		TService serviceAds = services.GetRequiredService<TService>();
		await serviceAds.Add(entity);
	}
}