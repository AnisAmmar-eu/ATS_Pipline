using ApiADS.Notifications;
using Core.Entities.Alarms;
using Core.Entities.Alarms.AlarmsPLC.Models.DB;
using Core.Entities.Alarms.AlarmsPLC.Models.DTO;
using Core.Entities.Alarms.AlarmsPLC.Services;
using Microsoft.AspNetCore.Mvc;
using TwinCAT.Ads;

namespace ApiADS.Controllers;

public partial class ADSController : ControllerBase
{
	protected async Task<AlarmNotification>
		InitAlarmNotification(dynamic ads)
	{
		AdsClient tcClient = (AdsClient)ads.tcClient;

		uint alarmAcquit = tcClient.CreateVariableHandle(Utils.AcquitMsg);
		uint alarmNew = tcClient.CreateVariableHandle(Utils.NewMsg);
		uint alarmOldEntry = tcClient.CreateVariableHandle(Utils.ToRead);

		int size = sizeof(UInt32);
		ResultHandle alarmHandle = await tcClient.AddDeviceNotificationAsync(Utils.NewMsg, size,
			new NotificationSettings(AdsTransMode.OnChange, 0, 0), ads, ads.cancel);
		AlarmNotification notification =
			new(alarmHandle, alarmAcquit, alarmNew, alarmOldEntry);
		tcClient.AdsNotification += notification.GetElement;

		ResultValue<uint> newMsgValue = await tcClient.ReadAnyAsync<uint>(ads.msgNewHandle, ads.cancel);
		if (newMsgValue.ErrorCode != AdsErrorCode.NoError)
			throw new Exception(newMsgValue.ErrorCode.ToString());
		if (newMsgValue.Value == Utils.HasNewMsg)
			notification.GetElementSub(ads);
		return notification;
	}
}