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
    public int nbCandidats { get; set; }
    public int threshold { get; set; }
    public double MScore { get; set; }
    public double NMminScore { get; set; }
    public double NMmaxScore { get; set; }
    public double NMAvg { get; set; }
    public double NMStdev {  get; set; }
    public double computeTime {  get; set; }

    public List<TenBestMatch> tenBestMatches { get; set; } = new();
}