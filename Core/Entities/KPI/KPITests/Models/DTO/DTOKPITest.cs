using System.Globalization;
using Core.Entities.KPI.KPITests.Models.DB;
using Core.Shared.Models.DB.Kernel.Interfaces;
using Core.Shared.Models.DTO.Kernel;
using Core.Shared.Models.DTO.Kernel.Interfaces;

namespace Core.Entities.KPI.KPITests.Models.DTO;

public class DTOKPITest : DTOBaseEntity, IDTO<KPITest, DTOKPITest>, IBaseKPI<double>
{
	public DTOKPITest(KPITest kpiTest)
	{
		ID = kpiTest.ID;
		TS = kpiTest.TS;
		Value = kpiTest.Value;
	}

	public double Value { get; set; }

	public double GetValue()
	{
		return Value;
	}

	public string[] GetKPICRID()
	{
		return new[] { "KPITest", "KPITestMax" };
	}

	public Func<List<double>, string[]> GetComputedValues()
	{
		return doubleList =>
		{
			return new[]
			{
				doubleList.Any() ? doubleList.Average().ToString(CultureInfo.InvariantCulture) : "0",
				doubleList.Any() ? doubleList.Max().ToString(CultureInfo.InvariantCulture) : "0"
			};
		};
	}
}