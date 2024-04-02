using Core.Entities.Vision.ToDos.Dictionaries;
using Core.Entities.Vision.ToDos.Models.DB.ToNotifys;
using Core.Entities.Vision.ToDos.Models.DTO.ToNotifys;
using Core.Shared.Data;
using Core.Shared.Exceptions;
using Core.Shared.Repositories.Kernel;

namespace Core.Entities.Vision.ToDos.Repositories.ToNotifys;

public class ToNotifyRepository : BaseEntityRepository<AnodeCTX, ToNotify, DTOToNotify>,
	IToNotifyRepository
{
	public ToNotifyRepository(AnodeCTX context) : base(context, [], [])
	{
	}
}