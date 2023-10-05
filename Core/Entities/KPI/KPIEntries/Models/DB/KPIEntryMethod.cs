using Core.Entities.KPI.KPIEntries.Models.DTO;
using Core.Shared.Models.DB.Kernel;
using Core.Shared.Models.DB.Kernel.Interfaces;

namespace Core.Entities.KPI.KPIEntries.Models.DB;

public partial class KPIEntry : BaseEntity, IBaseEntity<KPIEntry, DTOKPIEntry>
{
	public override DTOKPIEntry ToDTO()
	{
		return new DTOKPIEntry(this);
	}
}