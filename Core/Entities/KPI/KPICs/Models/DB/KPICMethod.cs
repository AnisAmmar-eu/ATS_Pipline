using Core.Entities.KPI.KPICs.Models.DTO;
using Core.Shared.Models.DB.Kernel;
using Core.Shared.Models.DB.Kernel.Interfaces;

namespace Core.Entities.KPI.KPICs.Models.DB;

public partial class KPIC : BaseEntity, IBaseEntity<KPIC, DTOKPIC>
{
	public KPIC()
	{
	}

	public KPIC(string rid, string name, string description)
	{
		RID = rid;
		Name = name;
		Description = description;
	}

	public override DTOKPIC ToDTO()
	{
		return new DTOKPIC(this);
	}
}