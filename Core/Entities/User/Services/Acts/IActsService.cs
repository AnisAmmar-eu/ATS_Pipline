using Core.Entities.User.Models.DTO.Acts.ActEntities;
using Microsoft.AspNetCore.Http;

namespace Core.Entities.User.Services.Acts;

public interface IActsService
{
	Task<List<DTOActEntityStatus>> ActionsFromList(HttpContext httpContext,
		List<DTOActEntityStatus> dtoActEntitiesStatus);

	Task<DTOActEntityStatus> GetAction(HttpContext httpContext, DTOActEntityStatus dtoActEntityStatus);
	Task<DTOActEntity> GetActionEntityRoles(DTOActEntity dtoActEntity);
	Task AssignAction(DTOActEntity dtoActEntity, bool remove = true);

	Task DeleteActionEntity(string actRID, string? entityType = null, int? entityID = null, string? parentType = null,
		int? parentID = null);

	Task<bool> DuplicateActionEntities(DTOActEntityToValid dtoActToDuplicate, DTOActEntityToValid dtoAct);
	Task<string> HasRights(HttpContext httpContext, DTOActEntityToValid dtoActEntityToValid);
	bool ValidActionToken(HttpContext httpContext, List<DTOActEntityToValid> dtoActEntitiesToValid);
}