using Core.Entities.KPIData.KPIs.Models.DTO;
using Core.Entities.KPIData.TenBestMatchs.Models.DB;
using Core.Entities.StationCycles.Models.DB;
using Core.Entities.Vision.ToDos.Models.DTO;
using Core.Shared.Models.DB.Kernel;
using Core.Shared.Models.DB.Kernel.Interfaces;
using System.Security.Claims;

namespace Core.Entities.KPIData.KPIs.Models.DB;

public partial class KPI : BaseEntity, IBaseEntity<KPI, DTOKPI>
{
    public int NbCandidats { get; set; }
    public int Threshold { get; set; }
    public double MScore { get; set; }
    public double NMminScore { get; set; }
    public double NMmaxScore { get; set; }
    public double NMAvg { get; set; }
    public double NMStdev {  get; set; }
    public double ComputeTime {  get; set; }

    public StationCycle StationCycle { get; set; }
    public List<TenBestMatch> TenBestMatches { get; set; } = new();
}