using Core.Entities.Alarms;
using Core.Entities.Alarms.AlarmsLog.Models.DB;
using Core.Entities.Alarms.AlarmsLog.Models.DTO;
using Core.Entities.Alarms.AlarmsLog.Services;
using Core.Entities.Alarms.AlarmsPLC.Models.DB;
using Core.Entities.Alarms.AlarmsPLC.Models.DTO;
using Core.Entities.Alarms.AlarmsPLC.Services;
using Core.Entities.Packets.Services;
using Core.Shared.Dictionaries;
using Core.Shared.SignalR.StationCycleHub;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Shared.Services.Notifications;

public class AlarmNotification : BaseNotification<Alarm, Alarm>
{
	public static async Task<AlarmNotification> Create(dynamic ads)
	{
		return await CreateSub<AlarmNotification>(ads, ADSUtils.AlarmRemove, ADSUtils.AlarmNewMsg,
			ADSUtils.AlarmToRead);
	}

	protected override async Task AddElement(IServiceProvider services, Alarm alarm)
	{
		IAlarmLogService alarmLogService = services.GetRequiredService<IAlarmLogService>();
		await alarmLogService.Collect(alarm);
	}
}