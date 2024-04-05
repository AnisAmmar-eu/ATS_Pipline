using Core.Entities.KPIData.KPIs.Models.DB;
using Core.Entities.KPIData.TenBestMatchs.Models.DTO;
using Core.Entities.Packets.Models.DB.AlarmLists;
using Core.Entities.StationCycles.Models.DB;
using Core.Entities.Vision.ToDos.Models.DTO;
using Core.Migrations;
using Core.Shared.Models.DB.Kernel;
using Core.Shared.Models.DB.Kernel.Interfaces;

namespace Core.Entities.KPIData.TenBestMatchs.Models.DB;

public partial class TenBestMatch : BaseEntity, IBaseEntity<TenBestMatch, DTOTenBestMatch>
{
    public int rank { get; set; }
    public string anodeID { get; set; }
    public int score { get; set; }

    public int KPIID { get; set; }

    private KPI? _KPI;

    public KPI KPI
    {
        set => _KPI = value;
        get => _KPI
            ?? throw new InvalidOperationException("Uninitialized property: " + nameof(AlarmList));
    }
}