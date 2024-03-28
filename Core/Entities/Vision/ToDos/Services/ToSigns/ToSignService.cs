using Core.Entities.Vision.ToDos.Dictionaries;
using Core.Entities.Vision.ToDos.Models.DB.ToLoads;
using Core.Entities.Vision.ToDos.Models.DB.ToSigns;
using Core.Entities.Vision.ToDos.Models.DTO.ToSigns;
using Core.Entities.Vision.ToDos.Repositories.ToSigns;
using Core.Shared.Services.Kernel;
using Core.Shared.UnitOfWork.Interfaces;

namespace Core.Entities.Vision.ToDos.Services.ToSigns;

public class ToSignService : BaseEntityService<IToSignRepository, ToSign, DTOToSign>,
	IToSignService
{
	public ToSignService(IAnodeUOW anodeUOW) : base(anodeUOW)
	{
	}
}