using Core.Entities.Vision.ToDos.Dictionaries;
using Core.Entities.Vision.ToDos.Models.DB.ToLoads;
using Core.Entities.Vision.ToDos.Models.DTO.ToLoads;
using Core.Entities.Vision.ToDos.Repositories.ToLoads;
using Core.Shared.Services.Kernel;
using Core.Shared.UnitOfWork.Interfaces;

namespace Core.Entities.Vision.ToDos.Services.ToLoads;

public class ToLoadService : BaseEntityService<IToLoadRepository, ToLoad, DTOToLoad>,
	IToLoadService
{
	public ToLoadService(IAnodeUOW anodeUOW) : base(anodeUOW)
	{
	}
}