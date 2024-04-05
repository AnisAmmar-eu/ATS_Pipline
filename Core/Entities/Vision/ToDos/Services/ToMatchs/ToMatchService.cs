using Core.Entities.Vision.ToDos.Models.DB.ToMatchs;
using Core.Entities.Vision.ToDos.Models.DTO.ToMatchs;
using Core.Entities.Vision.ToDos.Repositories.ToMatchs;
using Core.Shared.Services.Kernel;
using Core.Shared.UnitOfWork.Interfaces;

namespace Core.Entities.Vision.ToDos.Services.ToMatchs;

public class ToMatchService :
	BaseEntityService<IToMatchRepository, ToMatch, DTOToMatch>,
	IToMatchService
{
	public ToMatchService(IAnodeUOW anodeUOW) : base(anodeUOW)
	{
	}
}