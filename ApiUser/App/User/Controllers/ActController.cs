using Carter;
using Core.Entities.User.Dictionaries;
using Core.Entities.User.Models.DB.Acts;
using Core.Entities.User.Models.DTO.Acts;
using Core.Entities.User.Models.DTO.Acts.ActEntities;
using Core.Entities.User.Services.Acts;
using Core.Shared.Endpoints.Kernel;
using Core.Shared.Models.ApiResponses;
using Core.Shared.Services.System.Logs;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ApiUser.App.User.Controllers;

/// <summary>
///     Acts Routes
/// </summary>
public class ActController : BaseEndpoint<Act, DTOAct, IActService>, ICarterModule
{
	/// <summary>
	///     Add Routes from CarterModule
	/// </summary>
	/// <param name="app"></param>
	public void AddRoutes(IEndpointRouteBuilder app)
	{
		RouteGroupBuilder
			group = app.MapGroup("apiUser/acts").WithTags(nameof(ActController)).RequireAuthorization();

		group.MapPost("hasRights", HasRights);
		group.MapPost("entity", GetActEntityWithRoles);
		group.MapPost("list", GetActEntitiesStatusFromList);
		group.MapPut("assign", AssignActionsFromList).RequireAuthorization(ActionRID.ADMIN_GENERAL_RIGHTS);
	}

	// POST apiUser/acts/hasRights
	/// <summary>
	///     Check if a user has the rights to do an action
	/// </summary>
	/// <param name="dtoActEntityToValid"></param>
	/// <param name="actService"></param>
	/// <param name="logService"></param>
	/// <param name="httpContext"></param>
	/// <returns>The action token as string</returns>
	private static async Task<JsonHttpResult<ApiResponse>> HasRights([FromBody] DTOActEntityToValid dtoActEntityToValid,
		IActService actService, ILogService logService, HttpContext httpContext)
	{
		return await GenericController(async () =>
		{
			string actionToken = await actService.HasRights(httpContext, dtoActEntityToValid);
			return new { actionToken };
		}, logService, httpContext);
	}

	// POST apiUser/acts/entity
	/// <summary>
	///     Get an actEntity with roles and users
	/// </summary>
	/// <param name="dtoActEntity"></param>
	/// <param name="actService"></param>
	/// <param name="logService"></param>
	/// <param name="httpContext"></param>
	/// <returns>A <see cref="DTOActEntity" /></returns>
	private static async Task<JsonHttpResult<ApiResponse>> GetActEntityWithRoles([FromBody] DTOActEntity dtoActEntity,
		IActService actService, ILogService logService, HttpContext httpContext)
	{
		return await GenericController(async () => await actService.GetActionEntityRoles(dtoActEntity), logService,
			httpContext);
	}

	// POST apiUser/acts/list
	/// <summary>
	///     Retreive all actEntities status from a given list
	/// </summary>
	/// <param name="dtoActEntitiesStatus"></param>
	/// <param name="actService"></param>
	/// <param name="logService"></param>
	/// <param name="httpContext"></param>
	/// <returns>A <see cref="List{DTOActEntityStatus}" /></returns>
	private static async Task<JsonHttpResult<ApiResponse>> GetActEntitiesStatusFromList(
		[FromBody] List<DTOActEntityStatus> dtoActEntitiesStatus, IActService actService, ILogService logService,
		HttpContext httpContext)
	{
		return await GenericController(async () => await actService.ActionsFromList(httpContext, dtoActEntitiesStatus),
			logService, httpContext);
	}

	// PUT apiUser/acts/assign
	/// <summary>
	///     Assign acts from MANAGE
	/// </summary>
	/// <param name="dtoActEntities"></param>
	/// <param name="actService"></param>
	/// <param name="logService"></param>
	/// <param name="httpContext"></param>
	/// <returns></returns>
	/// <exception cref="UnauthorizedAccessException"></exception>
	private static async Task<JsonHttpResult<ApiResponse>> AssignActionsFromList(
		[FromBody] List<DTOActEntity> dtoActEntities, IActService actService, ILogService logService,
		HttpContext httpContext)
	{
		return await GenericControllerEmptyResponse(async () =>
		{
			foreach (DTOActEntity dtoActEntity in dtoActEntities.Where(dto =>
				         dto.Act != null && dto.Act.RID.StartsWith("MANAGE.")))
				await actService.AssignAction(dtoActEntity);
		}, logService, httpContext);
	}
}