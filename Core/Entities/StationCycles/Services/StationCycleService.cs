using Core.Entities.Alarms.AlarmsLog.Models.DTO.DTOS;
using Core.Entities.StationCycles.Models.DB;
using Core.Entities.StationCycles.Models.DTO;
using Core.Entities.StationCycles.Repositories;
using Core.Shared.Services.Kernel;
using Core.Shared.UnitOfWork.Interfaces;

namespace Core.Entities.StationCycles.Services;

public class StationCycleService : ServiceBaseEntity<IStationCycleRepository, StationCycle, DTOStationCycle>
{
	public StationCycleService(IAnodeUOW anodeUOW) : base(anodeUOW)
	{
	}
}