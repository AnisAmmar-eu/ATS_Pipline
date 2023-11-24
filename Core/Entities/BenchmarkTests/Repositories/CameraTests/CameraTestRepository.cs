using Core.Entities.BenchmarkTests.Models.DB.CameraTests;
using Core.Entities.BenchmarkTests.Models.DTO.CameraTests;
using Core.Shared.Data;
using Core.Shared.Repositories.Kernel;

namespace Core.Entities.BenchmarkTests.Repositories.CameraTests;

public class CameraTestRepository : BaseEntityRepository<AnodeCTX, CameraTest, DTOCameraTest>, ICameraTestRepository
{
	public CameraTestRepository(AnodeCTX context) : base(context)
	{
	}
}