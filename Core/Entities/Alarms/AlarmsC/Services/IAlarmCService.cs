using Core.Entities.Alarms.AlarmsC.Models.DTO;

namespace Core.Entities.Alarms.AlarmsC.Services;

public interface IAlarmCService
{
	public Task<List<DTOAlarmC>> GetAll();
	public Task<DTOAlarmC> GetById(int ID);
	public Task<DTOAlarmC> GetByRID(string RID);
	public Task<DTOAlarmC> AddReceivedAlarmC(DTOAlarmC received);
}