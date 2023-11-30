using Core.Entities.BenchmarkTests.Models.DTO.CameraTests;
using Core.Shared.Models.DB.Kernel;
using Core.Shared.Models.DB.Kernel.Interfaces;

namespace Core.Entities.BenchmarkTests.Models.DB.CameraTests;

public class CameraTest : BaseEntity, IBaseEntity<CameraTest, DTOCameraTest>
{
	public override DTOCameraTest ToDTO()
	{
		return new(this);
	}
}