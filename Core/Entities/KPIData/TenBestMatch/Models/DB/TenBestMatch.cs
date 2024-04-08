using Core.Entities.KPIData.KPIs.Models.DB;
using Core.Entities.KPIData.TenBestMatchs.Models.DTO;
using Core.Shared.Models.DB.Kernel;
using Core.Shared.Models.DB.Kernel.Interfaces;

namespace Core.Entities.KPIData.TenBestMatchs.Models.DB;

public partial class TenBestMatch : BaseEntity, IBaseEntity<TenBestMatch, DTOTenBestMatch>
{
    public int Rank { get; set; }
    public string AnodeID { get; set; }
    public int Score { get; set; }

    public int KPIID { get; set; }

    private KPI? _KPI;

    public KPI KPI
    {
        set => _KPI = value;
        get => _KPI
        	?? throw new InvalidOperationException("Uninitialized property: " + nameof(KPI));
    }
}