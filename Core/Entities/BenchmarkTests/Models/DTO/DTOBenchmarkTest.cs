using Core.Entities.BenchmarkTests.Models.DB;
using Core.Entities.BenchmarkTests.Models.DTO.CameraTests;
using Core.Shared.Dictionaries;
using Core.Shared.Models.DTO.Kernel;
using Core.Shared.Models.DTO.Kernel.Interfaces;

namespace Core.Entities.BenchmarkTests.Models.DTO;

public class DTOBenchmarkTest : DTOBaseEntity, IDTO<BenchmarkTest, DTOBenchmarkTest>
{
	public DTOBenchmarkTest()
	{
	}

	public DTOBenchmarkTest(BenchmarkTest benchmarkTest) : base(benchmarkTest)
	{
		RID = benchmarkTest.RID;
		CameraID = benchmarkTest.CameraID;
		Status = benchmarkTest.Status;
		StationID = benchmarkTest.StationID;
		AnodeType = benchmarkTest.AnodeType;
	}

	public string RID { get; set; } = string.Empty;
	public int CameraID { get; set; }
	public int StationID { get; set; }
	public int Status { get; set; }
	public int AnodeType { get; set; }


	public override BenchmarkTest ToModel()
	{
		return new BenchmarkTest(this);
	}
}