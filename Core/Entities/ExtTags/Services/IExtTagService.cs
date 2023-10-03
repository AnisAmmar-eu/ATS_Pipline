using Core.Entities.ExtTags.Models.DB;
using Core.Entities.ExtTags.Models.DTO;
using Core.Shared.Services.Kernel.Interfaces;

namespace Core.Entities.ExtTags.Services;

public interface IExtTagService : IServiceBaseEntity<ExtTag, DTOExtTag>
{
	
}