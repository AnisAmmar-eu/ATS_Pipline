using Core.Entities.Alarms;
using Core.Entities.Alarms.AlarmsLog.Models.DB;
using Core.Entities.Alarms.AlarmsLog.Services;
using Core.Shared.Dictionaries;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Core.Shared.Services.Notifications;

/// <summary>
/// Creates the <see cref="BaseNotification{T,TStruct}"/> for the alarm queue.
/// </summary>
public class AlarmNotification : BaseNotification<Alarm, Alarm>
{
	public static async Task<AlarmNotification> Create(dynamic ads, ILogger logger)
	{
		return await CreateSub<AlarmNotification>(
			ads,
			ADSUtils.AlarmRemove,
			ADSUtils.AlarmNewMsg,
			ADSUtils.AlarmAcquitMsg,
			ADSUtils.AlarmToRead,
			logger);
	}

	/// <summary>
	/// Instead of adding it to the table, it calls the collect method which updates the <see cref="AlarmLog"/> table./>
	/// </summary>
	/// <param name="services"></param>
	/// <param name="entity"></param>
	/// <returns></returns>
	protected override Task AddElement(IServiceProvider services, Alarm entity)
	{
		IAlarmLogService alarmLogService = services.GetRequiredService<IAlarmLogService>();
		return alarmLogService.Collect(entity);
	}
}