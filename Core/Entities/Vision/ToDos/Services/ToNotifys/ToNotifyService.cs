using Core.Entities.Vision.ToDos.Models.DB.ToNotifys;
using Core.Entities.Vision.ToDos.Models.DTO.ToNotifys;
using Core.Entities.Vision.ToDos.Repositories.ToNotifys;
using Core.Shared.Services.Kernel;
using Core.Shared.UnitOfWork.Interfaces;

namespace Core.Entities.Vision.ToDos.Services.ToNotifys;

public class ToNotifyService :
	BaseEntityService<IToNotifyRepository, ToNotify, DTOToNotify>,
	IToNotifyService
{
	public ToNotifyService(IAnodeUOW anodeUOW) : base(anodeUOW)
	{
	}
}