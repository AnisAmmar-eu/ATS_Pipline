using Core.Entities.KPI.KPICs.Models.DB;

namespace Core.Entities.KPI.KPICs.Models.DTO;

public partial class DTOKPIC
{
	public DTOKPIC(KPIC kpic)
	{
		RID = kpic.RID;
		Name = kpic.Name;
		Description = kpic.Description;
	}

	public override KPIC ToModel()
	{
		return new KPIC(this);
	}
}