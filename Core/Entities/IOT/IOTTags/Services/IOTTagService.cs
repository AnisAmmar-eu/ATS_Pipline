using Core.Entities.IOT.IOTTags.Models.DB;
using Core.Entities.IOT.IOTTags.Models.DTO;
using Core.Entities.IOT.IOTTags.Repositories;
using Core.Shared.Services.Kernel;
using Core.Shared.UnitOfWork.Interfaces;

namespace Core.Entities.IOT.IOTTags.Services;

public class IOTTagService : ServiceBaseEntity<IIOTTagRepository, IOTTag, DTOIOTTag>, IIOTTagService
{
	public IOTTagService(IAnodeUOW anodeUOW) : base(anodeUOW)
	{
	}
}