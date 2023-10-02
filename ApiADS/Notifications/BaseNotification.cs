using System.Buffers.Binary;
using Core.Entities.Alarms;
using Core.Entities.Alarms.AlarmsPLC.Models.DB;
using Core.Entities.Alarms.AlarmsPLC.Services;
using Core.Shared.Models.DB.Kernel.Interfaces;
using Core.Shared.Models.DTOs.Kernel.Interfaces;
using Core.Shared.Services.Kernel.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using TwinCAT.Ads;

namespace ApiADS.Notifications;

public class BaseNotification<TService, T, TDTO, TStruct>
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
		AdsClient? tcClient = (dynamicObject!.tcClient as AdsClient);
		tcClient!.WriteAny(_acquitMsg, Utils.IsReading);
		tcClient.WriteAny(_newMsg, Utils.NoMessage);
		// Get element of FIFO
		TStruct adsStruct = (TStruct)tcClient.ReadAny(_oldEntry, typeof(TStruct));
		T entity = adsStruct.ToModel();
		await using AsyncServiceScope scope = ((IServiceProvider)dynamicObject.appServices).CreateAsyncScope();
		IServiceProvider services = scope.ServiceProvider;

		try
		{
			var serviceAds = services.GetRequiredService<TService>();
			await serviceAds.Add(entity);
			tcClient.WriteAny(_acquitMsg, Utils.FinishedReading);
		}
		catch
		{
			// Retry
			tcClient.WriteAny(_acquitMsg, Utils.ErrorWhileReading);
		}
	}
}