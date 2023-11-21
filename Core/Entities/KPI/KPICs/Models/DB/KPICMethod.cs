using Core.Entities.KPI.KPICs.Models.DTO;

namespace Core.Entities.KPI.KPICs.Models.DB;

public partial class KPIC
{
	public KPIC()
	{
	}

	public KPIC(DTOKPIC dtoKPIC) : base(dtoKPIC)
	{
		RID = dtoKPIC.RID;
		Name = dtoKPIC.Name;
		Description = dtoKPIC.Description;
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