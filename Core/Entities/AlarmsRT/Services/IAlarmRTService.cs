using Core.Entities.AlarmsRT.Models.DTO;

namespace Core.Entities.AlarmsRT.Services;

public interface IAlarmRTService
{
	public Task<List<DTOAlarmRT>> GetAll();
}