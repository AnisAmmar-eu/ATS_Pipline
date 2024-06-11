using Core.Entities.Vision.Testing.Models.DB;
using Core.Entities.Vision.Testing.Models.DTO;
using Core.Entities.Vision.Testing.Repositories;
using Core.Entities.Vision.ToDos.Repositories.ToSigns;
using Core.Shared.Services.Kernel;
using Core.Shared.UnitOfWork.Interfaces;

namespace Core.Entities.Vision.Testing.Services;

public class StationTestService :
	BaseEntityService<IStationTestRepository, StationTest, DTOStationTest>,
	IStationTestService
{
	public StationTestService(IAnodeUOW anodeUOW) : base(anodeUOW)
	{
	}
}