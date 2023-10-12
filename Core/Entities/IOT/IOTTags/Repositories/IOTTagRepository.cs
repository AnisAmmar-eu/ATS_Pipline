using Core.Entities.IOT.IOTTags.Models.DB;
using Core.Entities.IOT.IOTTags.Models.DTO;
using Core.Shared.Data;
using Core.Shared.Repositories.Kernel;

namespace Core.Entities.IOT.IOTTags.Repositories;

public class IOTTagRepository : RepositoryBaseEntity<AnodeCTX, IOTTag, DTOIOTTag>, IIOTTagRepository
{
	public IOTTagRepository(AnodeCTX context) : base(context)
	{
	}
}