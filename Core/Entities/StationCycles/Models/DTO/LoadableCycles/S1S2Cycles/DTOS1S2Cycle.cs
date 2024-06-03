using Core.Entities.StationCycles.Dictionaries;
using Core.Entities.StationCycles.Models.DB.LoadableCycles.S1S2Cycles;
using Core.Shared.Models.DTO.Kernel.Interfaces;

namespace Core.Entities.StationCycles.Models.DTO.LoadableCycles.S1S2Cycles;

public partial class DTOS1S2Cycle : DTOLoadableCycle, IDTO<S1S2Cycle, DTOS1S2Cycle>
{
	new public string CycleType { get; set; } = CycleTypes.S1S2;
}