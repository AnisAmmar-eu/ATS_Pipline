using System.Buffers.Binary;
using Core.Entities.Alarms;
using Core.Entities.Alarms.AlarmsPLC.Models.DB;
using Core.Entities.Alarms.AlarmsPLC.Services;
using Microsoft.Extensions.DependencyInjection;
using TwinCAT.Ads;

namespace ApiADS.Notifications;

public class AlarmNotification
{
	private readonly ResultHandle _resultHandle;

	public AlarmNotification(ResultHandle resultHandle)
	{
		_resultHandle = resultHandle;
	}

	public void AlarmGetElement(object? sender, AdsNotificationEventArgs e)
	{
		uint newMsg = BinaryPrimitives.ReadUInt32LittleEndian(e.Data.Span);
		if (e.Handle == _resultHandle.Handle && newMsg == Utils.HasNewMsg)
		{
			Console.WriteLine("Notif msgNew");
			// UserData is our data passed in parameter
			AlarmGetElementSub(e.UserData as dynamic);
		}
	}
	
	public async void AlarmGetElementSub(dynamic dynamicObject)
	{
		AdsClient? tcClient = (dynamicObject!.tcClient as AdsClient);
		tcClient!.WriteAny((uint)dynamicObject.alarmAcquit, Utils.IsReading);
		tcClient.WriteAny((uint)dynamicObject.alarmNew, Utils.NoMessage);
		// Get element of FIFO
		Alarm alarm = (Alarm)tcClient.ReadAny((uint)dynamicObject.oldEntryHandle, typeof(Alarm));
		AlarmPLC alarmPLC = new(alarm);
		await using AsyncServiceScope scope = ((IServiceProvider)dynamicObject.appServices).CreateAsyncScope();
		IServiceProvider services = scope.ServiceProvider;

		try
		{
			var serviceAds = services.GetRequiredService<IAlarmPLCService>();
			await serviceAds.AddAlarmPLC(alarmPLC);
			tcClient.WriteAny((uint)dynamicObject.alarmAcquit, Utils.FinishedReading);
		}
		catch
		{
			// Retry
			tcClient.WriteAny((uint)dynamicObject.alarmAcquit, Utils.ErrorWhileReading);
		}
	}
}