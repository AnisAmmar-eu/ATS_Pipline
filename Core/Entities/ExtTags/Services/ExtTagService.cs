using Core.Entities.ExtTags.Models.DB;
using Core.Entities.ExtTags.Models.DTO;
using Core.Entities.ExtTags.Repositories;
using Core.Shared.Services.Kernel;
using Core.Shared.UnitOfWork.Interfaces;

namespace Core.Entities.ExtTags.Services;

public class ExtTagService : ServiceBaseEntity<IExtTagRepository, ExtTag, DTOExtTag>, IExtTagService
{
	protected ExtTagService(IAlarmUOW alarmUOW) : base(alarmUOW)
	{
	}
}