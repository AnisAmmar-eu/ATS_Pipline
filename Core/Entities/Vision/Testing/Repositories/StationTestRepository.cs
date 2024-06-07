using Core.Entities.Vision.Testing.Models.DB;
using Core.Entities.Vision.Testing.Models.DTO;
using Core.Shared.Data;
using Core.Shared.Repositories.Kernel;

namespace Core.Entities.Vision.Testing.Repositories;

public class StationTestRepository :
	BaseEntityRepository<AnodeCTX, StationTest, DTOStationTest>,
	IStationTestRepository
{
	public StationTestRepository(AnodeCTX context) : base(context, [], [])
	{
	}
}