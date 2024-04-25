using Core.Entities.KPIData.KPIs.Models.DB;
using Core.Entities.KPIData.TenBestMatchs.Models.DTO;
using Core.Shared.Models.DB.Kernel;
using Core.Shared.Models.DB.Kernel.Interfaces;

namespace Core.Entities.KPIData.TenBestMatchs.Models.DB;

public partial class TenBestMatch : BaseEntity, IBaseEntity<TenBestMatch, DTOTenBestMatch>
{
	public int Rank { get; set; }
	public string AnodeID { get; set; } = string.Empty;
	public int Score { get; set; }

	public int KPIID { get; set; }

	private KPI? _kpi;

	public KPI KPI
	{
		set => _kpi = value;
		get => _kpi
			?? throw new InvalidOperationException("Uninitialized property: " + nameof(KPI));
	}
}