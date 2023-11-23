using Core.Entities.BenchmarkTests.Models.DB.CameraTests;
using Core.Shared.Models.DTO.Kernel;
using Core.Shared.Models.DTO.Kernel.Interfaces;

namespace Core.Entities.BenchmarkTests.Models.DTO.CameraTests;

public class DTOCameraTest : DTOBaseEntity, IDTO<CameraTest, DTOCameraTest>
{
	public DTOCameraTest()
	{
	}

	public DTOCameraTest(CameraTest cameraTest) : base(cameraTest)
	{
	}

	public override CameraTest ToModel()
	{
		throw new NotImplementedException();
	}
}