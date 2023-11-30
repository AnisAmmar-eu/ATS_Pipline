using Core.Entities.User.Models.DB.Acts;
using Core.Entities.User.Models.DTO.Acts;
using Core.Shared.Repositories.Kernel.Interfaces;

namespace Core.Entities.User.Repositories.Acts;

public interface IActRepository : IBaseEntityRepository<Act, DTOAct>
{
	Task<Act> GetByRIDAndTypeWithIncludes(string? rid, string? entityType, string? parentType, bool withTracking = true);
}