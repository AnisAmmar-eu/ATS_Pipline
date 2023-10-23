using Core.Entities.StationCycles.Dictionaries;
using Core.Entities.StationCycles.Models.DB.S5Cycles;
using Core.Shared.Models.DTO.Kernel.Interfaces;

namespace Core.Entities.StationCycles.Models.DTO.S5Cycles;

public partial class DTOS5Cycle : DTOStationCycle, IDTO<S5Cycle, DTOS5Cycle>
{
	public DTOS5Cycle()
	{
		CycleType = CycleTypes.S5;
	}

	public DTOS5Cycle(S5Cycle s5Cycle) : base(s5Cycle)
	{
		CycleType = CycleTypes.S5;
	}
}