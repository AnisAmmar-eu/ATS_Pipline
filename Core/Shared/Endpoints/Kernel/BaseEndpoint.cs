using Core.Shared.Endpoints.Kernel.Dictionaries;
using Core.Shared.Models.ApiResponses;
using Core.Shared.Models.DB.Kernel.Interfaces;
using Core.Shared.Models.DTO.Kernel.Interfaces;
using Core.Shared.Paginations;
using Core.Shared.Services.Kernel.Interfaces;
using Core.Shared.Services.System.Logs;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace Core.Shared.Endpoints.Kernel;

public class BaseEndpoint<T, TDTO, TService>
	where T : class, IBaseEntity<T, TDTO>
	where TDTO : class, IDTO<T, TDTO>
	where TService : IServiceBaseEntity<T, TDTO>
{
	protected void AddBaseEndpoints(RouteGroupBuilder group, BaseEndpointFlags flags)
	{
		if ((flags & BaseEndpointFlags.Create) == BaseEndpointFlags.Create)
		{
			group.MapPost("", AddAll)
				.WithSummary("Adds all the DTOs in the body to the database").WithOpenApi();
		}

		if ((flags & BaseEndpointFlags.Read) == BaseEndpointFlags.Read)
		{
			group.MapGet("", GetAll)
				.WithSummary("Gets all").WithOpenApi();
			group.MapPut("", GetAllWithIncludes)
				.WithSummary("Gets all with includes listed in body").WithOpenApi();

			group.MapGet("id/{id}", GetByID)
				.WithSummary("Gets by ID").WithOpenApi();
			group.MapPut("id/{id}", GetByIDWithIncludes)
				.WithSummary("Gets all by ID with includes listed in body").WithOpenApi();

			group.MapPut("pagination/{nbItems}/{lastID}", GetWithPagination)
				.WithSummary("Gets by paging, sorting and filtering with includes").WithOpenApi();
		}

		if ((flags & BaseEndpointFlags.Update) == BaseEndpointFlags.Update)
		{
			group.MapPut("update", UpdateAll)
				.WithSummary("Updates all DTOs in the body if they already exist").WithOpenApi();
		}

		if ((flags & BaseEndpointFlags.Delete) == BaseEndpointFlags.Delete)
		{
			group.MapDelete("", RemoveAll)
				.WithSummary("Removes all DTOs in the body").WithOpenApi();
		}
	}

	#region Add

	private static async Task<Results<Ok<ApiResponse>, JsonHttpResult<ApiResponse>>> AddAll(TService service,
		ILogService logService, HttpContext httpContext, [FromBody] List<TDTO> dtos)
	{
		return await GenericController(async () => await service.AddAll(dtos.ConvertAll(dto => dto.ToModel())),
			logService, httpContext);
	}

	#endregion

	#region Read

	private static async Task<Results<Ok<ApiResponse>, JsonHttpResult<ApiResponse>>> GetByID(TService service,
		ILogService logService, HttpContext httpContext, [FromRoute] int id)
	{
		return await GenericController(async () => await service.GetByID(id), logService,
			httpContext);
	}

	private static async Task<Results<Ok<ApiResponse>, JsonHttpResult<ApiResponse>>> GetByIDWithIncludes(
		TService service,
		ILogService logService, HttpContext httpContext, [FromRoute] int id, [FromBody] string[] includes)
	{
		return await GenericController(async () => await service.GetByID(id, includes: includes), logService,
			httpContext);
	}

	private static async Task<Results<Ok<ApiResponse>, JsonHttpResult<ApiResponse>>> GetAll(TService service,
		ILogService logService, HttpContext httpContext)
	{
		return await GenericController(async () => await service.GetAll(), logService,
			httpContext);
	}

	private static async Task<Results<Ok<ApiResponse>, JsonHttpResult<ApiResponse>>> GetAllWithIncludes(
		TService service,
		ILogService logService, HttpContext httpContext, [FromBody] string[] includes)
	{
		return await GenericController(async () => await service.GetAll(includes: includes), logService,
			httpContext);
	}

	private static async Task<Results<Ok<ApiResponse>, JsonHttpResult<ApiResponse>>> GetWithPagination(TService service,
		ILogService logService, HttpContext httpContext, [FromRoute] int nbItems, [FromRoute] int lastID,
		[FromBody] Pagination pagination)
	{
		return await GenericController(
			async () => await service.GetWithPagination(pagination, nbItems, lastID), logService,
			httpContext);
	}

	#endregion

	#region Update

	private static async Task<Results<Ok<ApiResponse>, JsonHttpResult<ApiResponse>>> UpdateAll(TService service,
		ILogService logService, HttpContext httpContext, [FromBody] List<TDTO> dtos)
	{
		return await GenericController(async () => await service.UpdateAll(dtos.ConvertAll(dto => dto.ToModel())),
			logService, httpContext);
	}

	#endregion

	#region Delete

	private static async Task<Results<Ok<ApiResponse>, JsonHttpResult<ApiResponse>>> RemoveAll(TService service,
		ILogService logService, HttpContext httpContext, [FromBody] List<TDTO> dtos)
	{
		return await GenericController(async () => await service.RemoveAll(dtos.ConvertAll(dto => dto.ToModel())),
			logService, httpContext);
	}

	#endregion
	protected static async Task<Results<Ok<ApiResponse>, JsonHttpResult<ApiResponse>>> GenericController<TReturn>(
		Func<Task<TReturn>> func,
		ILogService logService,
		HttpContext httpContext)
	{
		TReturn ans;
		try
		{
			ans = await func.Invoke();
		}
		catch (Exception e)
		{
			return await new ApiResponse().ErrorResult(logService, httpContext.GetEndpoint(), e);
		}

		return await new ApiResponse(ans).SuccessResult(logService, httpContext.GetEndpoint());
	}
}