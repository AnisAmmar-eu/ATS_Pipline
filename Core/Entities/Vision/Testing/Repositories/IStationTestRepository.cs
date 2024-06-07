using Core.Entities.Vision.Testing.Models.DB;
using Core.Entities.Vision.Testing.Models.DTO;
using Core.Shared.Repositories.Kernel.Interfaces;

namespace Core.Entities.Vision.Testing.Repositories;

public interface IStationTestRepository : IBaseEntityRepository<StationTest, DTOStationTest>
{
}