using Core.Entities.StationCycles.Models.DB;
using Core.Entities.StationCycles.Models.DTO;
using Core.Shared.Services.Kernel.Interfaces;

namespace Core.Entities.StationCycles.Services;

public interface IStationCycleService : IServiceBaseEntity<StationCycle, DTOStationCycle>
{
	
}