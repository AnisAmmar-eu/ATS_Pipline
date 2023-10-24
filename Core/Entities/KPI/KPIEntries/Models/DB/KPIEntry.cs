using Core.Entities.KPI.KPICs.Models.DB;
using Core.Entities.KPI.KPIEntries.Models.DTO;
using Core.Shared.Models.DB.Kernel;
using Core.Shared.Models.DB.Kernel.Interfaces;

namespace Core.Entities.KPI.KPIEntries.Models.DB;

public partial class KPIEntry : BaseEntity, IBaseEntity<KPIEntry, DTOKPIEntry>
{
	public int KPICID { get; set; }
	public int StationID { get; set; }
	public string Value { get; set; } = string.Empty;
	public string Period { get; set; } = string.Empty;

	#region Nav Properties

	private KPIC? _kpic;

	public KPIC KPIC
	{
		set => _kpic = value;
		get => _kpic
		       ?? throw new InvalidOperationException("Uninitialized property: " + nameof(KPIC));
	}

	#endregion
}