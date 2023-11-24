using Core.Entities.Alarms.AlarmsRT.Models.DB;
using Core.Entities.Alarms.AlarmsRT.Models.DTO;
using Core.Shared.Services.Kernel.Interfaces;

namespace Core.Entities.Alarms.AlarmsRT.Services;

public interface IAlarmRTService : IBaseEntityService<AlarmRT, DTOAlarmRT>
{
	/// <summary>
	///     This function returns the statistics of AlarmRT. The result will always be an array of length 3.
	/// </summary>
	/// <returns>
	///     res[0] => Nb No Alarms
	///     res[1] => Nb NonAck
	///     res[2] => Nb Active alarms;
	/// </returns>
	public Task<int[]> GetAlarmRTStats();
}