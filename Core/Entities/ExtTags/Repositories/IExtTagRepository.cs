using Core.Entities.ExtTags.Models.DB;
using Core.Entities.ExtTags.Models.DTO;
using Core.Shared.Repositories.Kernel.Interfaces;

namespace Core.Entities.ExtTags.Repositories;

public interface IExtTagRepository : IRepositoryBaseEntity<ExtTag, DTOExtTag>
{
	
}