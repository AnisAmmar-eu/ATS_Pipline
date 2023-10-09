using Core.Entities.ExtTags.Models.DB;
using Core.Entities.ExtTags.Models.DTO;
using Core.Shared.Data;
using Core.Shared.Repositories.Kernel;

namespace Core.Entities.ExtTags.Repositories;

public class ExtTagRepository : RepositoryBaseEntity<AnodeCTX, ExtTag, DTOExtTag>, IExtTagRepository
{
	public ExtTagRepository(AnodeCTX context) : base(context)
	{
	}
}