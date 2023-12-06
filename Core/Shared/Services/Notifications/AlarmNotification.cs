using Core.Entities.Alarms;
using Core.Entities.Alarms.AlarmsLog.Services;
using Core.Shared.Dictionaries;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Shared.Services.Notifications;

public class AlarmNotification : BaseNotification<Alarm, Alarm>
{
	public static async Task<AlarmNotification> Create(dynamic ads)
	{
		return await CreateSub<AlarmNotification>( ads, ADSUtils.AlarmRemove, ADSUtils.AlarmNewMsg, ADSUtils.AlarmToRead);
	}

	protected override Task AddElement(IServiceProvider services, Alarm entity)
	{
		IAlarmLogService alarmLogService = services.GetRequiredService<IAlarmLogService>();
		return alarmLogService.Collect(entity);
	}
}