using Core.Entities.StationCycles.Models.DTO.LoadableCycles;
using Core.Shared.Models.DB.Kernel.Interfaces;

namespace Core.Entities.StationCycles.Models.DB.LoadableCycles;

public abstract partial class LoadableCycle : StationCycle, IBaseEntity<LoadableCycle, DTOLoadableCycle>
{
}