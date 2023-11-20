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
	public string RID { get; set; } = string.Empty;
	public int CameraID { get; set; }
	public int StationID { get; set; }
	public string AnodeType { get; set; } = AnodeTypeDict.Undefined;

	#region NavProperties

	private CameraTest? _cameraTest { get; set; }

	public CameraTest CameraTest
	{
		set => _cameraTest = value;
		get => _cameraTest ?? throw new InvalidOperationException("Uninitialized property: " + nameof(CameraTest));
	}

	#endregion

	public override DTOBenchmarkTest ToDTO()
	{
		return new DTOBenchmarkTest(this);
	}
}