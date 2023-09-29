using System.Buffers.Binary;
using Core.Entities.Alarms;
using Core.Entities.Alarms.AlarmsPLC.Models.DB;
using Core.Entities.Alarms.AlarmsPLC.Services;
using Microsoft.Extensions.DependencyInjection;
using TwinCAT.Ads;

namespace ApiADS.Notifications;

public class AlarmNotification
{
	private readonly uint _acquitMsg;
	private readonly uint _newMsg;
	private readonly uint _oldEntry;
	private readonly ResultHandle _resultHandle;

	public AlarmNotification(ResultHandle resultHandle, uint acquitMsg, uint newMsg, uint oldEntry)
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
		Alarm alarm = (Alarm)tcClient.ReadAny(_oldEntry, typeof(Alarm));
		AlarmPLC alarmPLC = new(alarm);
		await using AsyncServiceScope scope = ((IServiceProvider)dynamicObject.appServices).CreateAsyncScope();
		IServiceProvider services = scope.ServiceProvider;

		try
		{
			var serviceAds = services.GetRequiredService<IAlarmPLCService>();
			await serviceAds.AddAlarmPLC(alarmPLC);
			tcClient.WriteAny(_acquitMsg, Utils.FinishedReading);
		}
		catch
		{
			// Retry
			tcClient.WriteAny(_acquitMsg, Utils.ErrorWhileReading);
		}
	}
}