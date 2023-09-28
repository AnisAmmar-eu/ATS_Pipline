using Core.Entities.Alarms.AlarmsRT.Models.DTO;

namespace Core.Entities.Alarms.AlarmsRT.Services;

public interface IAlarmRTService
{
	public Task<List<DTOAlarmRT>> GetAll();
}