using Core.Entities.KPI.KPITests.Models.DTO;
using Core.Shared.Models.DB.Kernel;
using Core.Shared.Models.DB.Kernel.Interfaces;

namespace Core.Entities.KPI.KPITests.Models.DB;

public class KPITest : BaseEntity, IBaseEntity<KPITest, DTOKPITest>
{
	public double Value { get; set; }

	public override DTOKPITest ToDTO()
	{
		return new DTOKPITest(this);
	}
}