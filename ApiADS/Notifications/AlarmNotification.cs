using Core.Entities.Alarms;
using Core.Entities.Alarms.AlarmsPLC.Models.DB;
using Core.Entities.Alarms.AlarmsPLC.Models.DTO;
using Core.Entities.Alarms.AlarmsPLC.Services;
using TwinCAT.Ads;

namespace ApiADS.Notifications;

public class AlarmNotification : BaseNotification<AlarmPLCService, AlarmPLC, DTOAlarmPLC, Alarm>
{
	public AlarmNotification(ResultHandle resultHandle, uint acquitMsg, uint newMsg, uint oldEntry)
		: base(resultHandle, acquitMsg, newMsg, oldEntry)
	{
	}
}