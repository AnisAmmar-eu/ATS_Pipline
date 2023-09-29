using ApiADS.Notifications;
using Microsoft.AspNetCore.Mvc;
using TwinCAT.Ads;

namespace ApiADS.Controllers;

public partial class ADSController : ControllerBase
{
	protected async Task<AlarmNotification> InitAlarmNotification(dynamic ads)
	{
        ads.alarmAcquit = ads.tcClient.CreateVariableHandle(Utils.AcquitMsg);
        ads.alarmNew = ads.tcClient.CreateVariableHandle(Utils.NewMsg);
        ads.alarmOldEntry = ads.tcClient.CreateVariableHandle(Utils.ToRead);

        AdsClient tcClient = (AdsClient)ads.tcClient;
        int size = sizeof(UInt32);
        ResultHandle alarmHandle = await tcClient.AddDeviceNotificationAsync(Utils.NewMsg, size, new NotificationSettings(AdsTransMode.OnChange, 0, 0), ads, ads.cancel);
        AlarmNotification alarmNotification = new AlarmNotification(alarmHandle);
        tcClient.AdsNotification += alarmNotification.AlarmGetElement;

        ResultValue<uint> newMsgValue = await tcClient.ReadAnyAsync<uint>(ads.msgNewHandle, ads.cancel);
        if (newMsgValue.ErrorCode != AdsErrorCode.NoError)
            throw new Exception(newMsgValue.ErrorCode.ToString());
        if (newMsgValue.Value == Utils.HasNewMsg)
	        alarmNotification.AlarmGetElementSub(ads);
        return alarmNotification;
	}
}