using Core.Entities.BenchmarkTests.Models.DB.CameraTests;
using Core.Entities.BenchmarkTests.Models.DTO.CameraTests;
using Core.Shared.Repositories.Kernel.Interfaces;

namespace Core.Entities.BenchmarkTests.Repositories.CameraTests;

public interface ICameraTestRepository : IBaseEntityRepository<CameraTest, DTOCameraTest>;