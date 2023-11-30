using Core.Entities.User.Models.DB.Acts;
using Core.Entities.User.Models.DTO.Acts;
using Core.Entities.User.Models.DTO.Acts.ActEntities;
using Core.Shared.Services.Kernel.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Core.Entities.User.Services.Acts;

public interface IActService : IBaseEntityService<Act, DTOAct>
{
	Task<List<DTOActEntityStatus>> ActionsFromList(HttpContext httpContext, List<DTOActEntityStatus> dtoActEntitiesStatus);

	Task<DTOActEntityStatus> GetAction(HttpContext httpContext, DTOActEntityStatus dtoActEntityStatus);
	Task<DTOActEntity> GetActionEntityRoles(DTOActEntity dtoActEntity);
	Task AssignAction(DTOActEntity dtoActEntity, bool remove = true);

	Task DeleteActionEntity(
		string actRID,
		string? entityType = null,
		int? entityID = null,
		string? parentType = null,
		int? parentID = null);

	Task<bool> DuplicateActionEntities(DTOActEntityToValid dtoActToDuplicate, DTOActEntityToValid dtoAct);
	Task<string> HasRights(HttpContext httpContext, DTOActEntityToValid dtoActEntityToValid);
	bool ValidActionToken(HttpContext httpContext, List<DTOActEntityToValid> dtoActEntitiesToValid);
}