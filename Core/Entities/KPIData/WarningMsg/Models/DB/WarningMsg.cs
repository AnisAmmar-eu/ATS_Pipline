using Core.Entities.KPIData.KPIs.Models.DB;
using Core.Entities.KPIData.WarningMsgs.Models.DTO;
using Core.Shared.Models.DB.Kernel;
using Core.Shared.Models.DB.Kernel.Interfaces;

namespace Core.Entities.KPIData.WarningMsgs.Models.DB;

public partial class WarningMsg : BaseEntity, IBaseEntity<WarningMsg, DTOWarningMsg>
{
	public int Code { get; set; }
	public string Message { get; set; }

	public int KPIID { get; set; }

	private KPI? _kpi;

	public KPI KPI
	{
		set => _kpi = value;
		get => _kpi
			?? throw new InvalidOperationException("Uninitialized property: " + nameof(KPI));
	}
}