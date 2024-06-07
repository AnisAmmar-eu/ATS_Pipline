using Core.Entities.Vision.Testing.Models.DB;
using Core.Entities.Vision.Testing.Models.DTO;
using Core.Shared.Services.Kernel.Interfaces;

namespace Core.Entities.Vision.Testing.Services;

public interface IStationTestService : IBaseEntityService<StationTest, DTOStationTest>
{
}