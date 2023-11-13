using Core.Entities.BenchmarkTests.Models.DB;
using Core.Shared.Dictionaries;
using Core.Shared.Models.DTO.Kernel;
using Core.Shared.Models.DTO.Kernel.Interfaces;

namespace Core.Entities.BenchmarkTests.Models.DTO;

public class DTOBenchmarkTest : DTOBaseEntity, IDTO<BenchmarkTest, DTOBenchmarkTest>
{
	public string RID { get; set; } = string.Empty;
	public int CameraID { get; set; }
	public int StationID { get; set; }
	public string AnodeType { get; set; } = AnodeTypeDict.Undefined;

	public DTOBenchmarkTest()
	{
		
	}
	public DTOBenchmarkTest(BenchmarkTest benchmarkTest)
	{
		RID = benchmarkTest.RID;
		CameraID = benchmarkTest.CameraID;
		StationID = benchmarkTest.StationID;
		AnodeType = benchmarkTest.AnodeType;
	}
}