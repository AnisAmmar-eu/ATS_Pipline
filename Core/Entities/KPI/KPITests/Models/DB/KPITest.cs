using System.Globalization;
using Core.Entities.KPI.KPITests.Models.DTO;
using Core.Shared.Models.DB.Kernel;
using Core.Shared.Models.DB.Kernel.Interfaces;

namespace Core.Entities.KPI.KPITests.Models.DB;

public class KPITest : BaseEntity, IBaseEntity<KPITest, DTOKPITest>, IBaseKPI<double>
{
	public double Value { get; set; }

	public override DTOKPITest ToDTO()
	{
		return new DTOKPITest(this);
	}

	public double GetValue()
	{
		return Value;
	}

	public static string[] GetKPICRID()
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