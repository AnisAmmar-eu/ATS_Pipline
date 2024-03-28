using Core.Entities.Vision.ToDos.Dictionaries;
using Core.Entities.Vision.ToDos.Models.DB.ToLoads;
using Core.Entities.Vision.ToDos.Models.DB.ToUnloads;
using Core.Entities.Vision.ToDos.Models.DTO.ToUnloads;
using Core.Entities.Vision.ToDos.Repositories.ToUnloads;
using Core.Shared.Services.Kernel;
using Core.Shared.UnitOfWork.Interfaces;

namespace Core.Entities.Vision.ToDos.Services.ToUnloads;

public class ToUnloadService : BaseEntityService<IToUnloadRepository, ToUnload, DTOToUnload>,
	IToUnloadService
{
	public ToUnloadService(IAnodeUOW anodeUOW) : base(anodeUOW)
	{
	}
}