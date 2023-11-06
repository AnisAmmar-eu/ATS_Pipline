using Core.Entities.Anodes.Models.DB;
using Core.Entities.Anodes.Models.DTO;
using Core.Entities.Anodes.Repositories;
using Core.Shared.Services.Kernel;
using Core.Shared.UnitOfWork.Interfaces;

namespace Core.Entities.Anodes.Services;

public class AnodeService : ServiceBaseEntity<IAnodeRepository, Anode, DTOAnode>, IAnodeService
{
	public AnodeService(IAnodeUOW anodeUOW) : base(anodeUOW)
	{
	}
}