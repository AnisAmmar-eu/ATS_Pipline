using Core.Entities.StationCycles.Models.DB.MatchableCycles;
using Core.Entities.Vision.SignedCycles.Models.DB.MatchableStacks;
using Core.Shared.Models.DTO.Kernel.Interfaces;

namespace Core.Entities.Vision.SignedCycles.Models.DTO.MatchableStacks;

public partial class DTOMatchableStack : DTOSignedCycle, IDTO<MatchableStack, DTOMatchableStack>
{
	public int MatchableCycleID { get; set; }
	public MatchableCycle? MatchableCycle { get; set; }
}