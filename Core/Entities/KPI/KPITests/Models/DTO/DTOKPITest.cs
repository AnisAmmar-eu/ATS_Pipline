using Core.Entities.KPI.KPITests.Models.DB;
using Core.Shared.Models.DTO.Kernel;
using Core.Shared.Models.DTO.Kernel.Interfaces;

namespace Core.Entities.KPI.KPITests.Models.DTO;

public class DTOKPITest : DTOBaseEntity, IDTO<KPITest, DTOKPITest>
{
	public DTOKPITest(KPITest kpiTest)
	{
		ID = kpiTest.ID;
		TS = kpiTest.TS;
		Value = kpiTest.Value;
	}

	public double Value { get; set; }
	public override KPITest ToModel()
	{
		return new KPITest(this);
	}
}