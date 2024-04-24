using Core.Entities.KPIData.KPIs.Models.DTO;
using Core.Entities.KPIData.TenBestMatchs.Models.DB;
using Core.Entities.StationCycles.Models.DB.MatchableCycles;
using Core.Shared.Models.DB.Kernel;
using Core.Shared.Models.DB.Kernel.Interfaces;

namespace Core.Entities.KPIData.KPIs.Models.DB;

public partial class KPI : BaseEntity, IBaseEntity<KPI, DTOKPI>
{
	public int NbCandidats { get; set; }
	public int Threshold { get; set; }
	public int MScore { get; set; }
	public int NMminScore { get; set; }
	public int NMmaxScore { get; set; }
	public double NMAvg { get; set; }
	public double NMStdev { get; set; }
	public long ComputeTime { get; set; }

	public MatchableCycle StationCycle { get; set; }
	public List<TenBestMatch> TenBestMatches { get; set; } = new();
}