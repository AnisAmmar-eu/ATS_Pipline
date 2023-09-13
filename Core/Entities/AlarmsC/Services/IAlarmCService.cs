using Core.Entities.AlarmsC.Models.DB;
using Core.Entities.AlarmsC.Models.DTO;

namespace Core.Entities.AlarmsC.Services;

public interface IAlarmCService
{
	public Task<DTOAlarmC> GetById(int ID);
	public Task<DTOAlarmC> GetByRID(string RID);
	public Task<DTOAlarmC> AddReceivedAlarmC(DTOAlarmC received);
}