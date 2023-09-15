using Core.Entities.AlarmsRT.Models.DTO;
using Core.Shared.UnitOfWork.Interfaces;

namespace Core.Entities.AlarmsRT.Services;

public class AlarmRTService : IAlarmRTService
{
	private readonly IAlarmUOW _alarmUOW;

	public AlarmRTService(IAlarmUOW alarmUOW)
	{
		_alarmUOW = alarmUOW;
	}

	public async Task<List<DTOAlarmRT>> GetAll()
	{
		return (await _alarmUOW.AlarmRT.GetAllWithInclude()).ConvertAll(alarmRT => alarmRT.ToDTO());
	}
}