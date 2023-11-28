using Core.Entities.BenchmarkTests.Models.DTO;
using Core.Shared.Models.DB.Kernel;
using Core.Shared.Models.DB.Kernel.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Core.Entities.BenchmarkTests.Models.DB;

[Index(nameof(RID))]
public class BenchmarkTest : BaseEntity, IBaseEntity<BenchmarkTest, DTOBenchmarkTest>
{
	public BenchmarkTest()
	{
	}

	public BenchmarkTest(DTOBenchmarkTest dtoBenchmarkTest) : base(dtoBenchmarkTest)
	{
		RID = dtoBenchmarkTest.RID;
		CameraID = dtoBenchmarkTest.CameraID;
		StationID = dtoBenchmarkTest.StationID;
		Status = dtoBenchmarkTest.Status;
		AnodeType = dtoBenchmarkTest.AnodeType;
	}

	public string RID { get; set; } = string.Empty;
	public int CameraID { get; set; }
	public int StationID { get; set; }
	public int Status { get; set; }
	public int AnodeType { get; set; }

	public override DTOBenchmarkTest ToDTO()
	{
		return new DTOBenchmarkTest(this);
	}
}