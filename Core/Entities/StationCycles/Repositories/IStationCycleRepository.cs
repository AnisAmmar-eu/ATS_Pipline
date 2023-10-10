using Core.Entities.StationCycles.Models.DB;
using Core.Entities.StationCycles.Models.DTO;
using Core.Shared.Repositories.Kernel.Interfaces;

namespace Core.Entities.StationCycles.Repositories;

public interface IStationCycleRepository : IRepositoryBaseEntity<StationCycle, DTOStationCycle>
{
}