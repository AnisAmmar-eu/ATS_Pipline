using Core.Entities.BenchmarkTests.Models.DTO;
using Core.Shared.Dictionaries;
using Core.Shared.Models.DB.Kernel;
using Core.Shared.Models.DB.Kernel.Interfaces;

namespace Core.Entities.BenchmarkTests.Models.DB;

public class BenchmarkTest : BaseEntity, IBaseEntity<BenchmarkTest, DTOBenchmarkTest>
{
	public string RID { get; set; } = string.Empty;
	public int CameraID { get; set; }
	public int StationID { get; set; }
	public string AnodeType { get; set; } = AnodeTypeDict.Undefined;

	public override DTOBenchmarkTest ToDTO()
	{
		return new DTOBenchmarkTest(this);
	}
}