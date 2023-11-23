using Core.Entities.BenchmarkTests.Models.DB.CameraTests;
using Core.Entities.BenchmarkTests.Models.DTO;
using Core.Shared.Dictionaries;
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
		CameraTest = dtoBenchmarkTest.CameraTest.ToModel();
		StationID = dtoBenchmarkTest.StationID;
		AnodeType = dtoBenchmarkTest.AnodeType;
	}

	public string RID { get; set; } = string.Empty;
	public int CameraID { get; set; }
	public int StationID { get; set; }
	public string AnodeType { get; set; } = AnodeTypeDict.Undefined;

	public override DTOBenchmarkTest ToDTO()
	{
		return new DTOBenchmarkTest(this);
	}

	#region NavProperties

	private CameraTest? CameraTestSub { get; set; }

	public CameraTest CameraTest
	{
		set => CameraTestSub = value;
		get => CameraTestSub ?? throw new InvalidOperationException("Uninitialized property: " + nameof(CameraTest));
	}

	#endregion
}