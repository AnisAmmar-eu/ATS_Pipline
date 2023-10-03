using Core.Entities.ExtTags.Models.DB;
using Core.Entities.ExtTags.Models.DTO;
using Core.Shared.Data;
using Core.Shared.Repositories.Kernel;

namespace Core.Entities.ExtTags.Repositories;

public class ExtTagRepository : RepositoryBaseEntity<AlarmCTX, ExtTag, DTOExtTag>, IExtTagRepository
{
	public ExtTagRepository(AlarmCTX context) : base(context)
	{
	}
}