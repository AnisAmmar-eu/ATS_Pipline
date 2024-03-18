using Core.Entities.StationCycles.Models.DB.MatchableCycles;
using Core.Entities.Vision.SignedCycles.Models.DB.ToMatchs;
using Core.Shared.Models.DTO.Kernel.Interfaces;

namespace Core.Entities.Vision.SignedCycles.Models.DTO.ToMatchs;

public partial class DTOToMatch : DTOSignedCycle, IDTO<ToMatch, DTOToMatch>
{
	public int MatchableCycleID { get; set; }
	public MatchableCycle? MatchableCycle { get; set; }
}