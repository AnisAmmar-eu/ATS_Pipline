using Core.Entities.User.Models.DB.Acts;
using Core.Entities.User.Models.DB.Acts.ActEntities;
using Core.Entities.User.Models.DTO.Acts.ActEntities;
using Core.Shared.Repositories.Kernel.Interfaces;

namespace Core.Entities.User.Repositories.Acts.ActEntities;

public interface IActEntityRepository : IBaseEntityRepository<ActEntity, DTOActEntity>
{
	Task<List<ActEntity>> GetAllByActWithIncludes(
		Act act,
		int? entityID,
		int? parentID,
		bool withTracking = true,
		int? maxCount = null);

	Task<ActEntity> GetByActWithIncludes(Act act, int? entityID, int? parentID, bool withTracking = true);
}