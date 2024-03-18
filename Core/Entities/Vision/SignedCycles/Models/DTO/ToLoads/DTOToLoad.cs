using Core.Entities.StationCycles.Models.DB.LoadableCycles;
using Core.Entities.Vision.SignedCycles.Models.DB.ToLoads;
using Core.Shared.Models.DTO.Kernel.Interfaces;

namespace Core.Entities.Vision.SignedCycles.Models.DTO.ToLoads;

public partial class DTOToLoad : DTOSignedCycle, IDTO<ToLoad, DTOToLoad>
{
	public int LoadableCycleID { get; set; }
	public LoadableCycle? LoadableCycle { get; set; }
}