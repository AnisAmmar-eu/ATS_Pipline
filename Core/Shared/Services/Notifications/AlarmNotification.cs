using Core.Entities.Alarms;
using Core.Entities.Alarms.AlarmsPLC.Models.DB;
using Core.Entities.Alarms.AlarmsPLC.Models.DTO;
using Core.Entities.Alarms.AlarmsPLC.Services;
using Core.Shared.Dictionaries;

namespace Core.Shared.Services.Notifications;

public class AlarmNotification : BaseNotification<IAlarmPLCService, AlarmPLC, DTOAlarmPLC, Alarm>
{
	public static async Task<AlarmNotification> Create(dynamic ads)
	{
		return await CreateSub<AlarmNotification>(ads, ADSUtils.AlarmAcquitMsg, ADSUtils.AlarmNewMsg,
			ADSUtils.AlarmToRead);
	}
}