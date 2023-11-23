using System.Reflection;
using System.Text.Json;
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

public class BaseEndpoint<T, TDTO, TService> : BaseController
	where T : class, IBaseEntity<T, TDTO>
	where TDTO : class, IDTO<T, TDTO>
	where TService : IServiceBaseEntity<T, TDTO>
{
	protected static void MapBaseEndpoints(RouteGroupBuilder group, BaseEndpointFlags flags)
	{
		string dtoName = typeof(TDTO).Name;
		string tName = typeof(T).Name;
		if ((flags & BaseEndpointFlags.Create) == BaseEndpointFlags.Create)
			group.MapPost("", Add)
				.WithSummary($"Add the {dtoName} in the body to the database").WithOpenApi();

		if ((flags & BaseEndpointFlags.Read) == BaseEndpointFlags.Read)
		{
			group.MapGet("", GetAll)
				.WithSummary($"Get all {tName}s").WithOpenApi();
			group.MapPut("", GetAllWithIncludes)
				.WithSummary($"Get all {tName}s with includes listed in body").WithOpenApi();

			group.MapGet("id/{id}", GetByID)
				.WithSummary($"Get a {tName} by ID").WithOpenApi();
			group.MapPut("id/{id}", GetByIDWithIncludes)
				.WithSummary($"Get all {tName}s by ID with includes listed in body").WithOpenApi();

			group.MapPut("pagination/{nbItems}/{lastID}", GetWithPagination)
				.WithSummary($"Get a {tName} by paging, sorting and filtering with includes").WithOpenApi();
		}

		if ((flags & BaseEndpointFlags.Update) == BaseEndpointFlags.Update)
			group.MapPut("update", Update)
				.WithSummary($"Update the {dtoName} in the body if it already exist").Accepts<TDTO>("application/json")
				.WithOpenApi();

		if ((flags & BaseEndpointFlags.Delete) == BaseEndpointFlags.Delete)
			group.MapDelete("", Remove)
				.WithSummary($"Remove the {dtoName} in the body").Accepts<TDTO>("application/json").WithOpenApi();
	}

	#region Add

	private static async Task<JsonHttpResult<ApiResponse>> Add(TService service, ILogService logService,
		HttpContext httpContext, TDTO dto)
	{
		return await GenericController(async () => await service.Add(dto.ToModel()), logService, httpContext);
	}

	#endregion

	#region Update

	private static async Task<JsonHttpResult<ApiResponse>> Update(TService service, ILogService logService,
		HttpContext httpContext, TDTO dto)
	{
		return await GenericController(async () => await service.Update(dto.ToModel()), logService, httpContext);
	}

	#endregion

	#region Delete

	// Here TDTO dto is NOT in the arguments as we need it from body with Custom Binding. It is therefore read in the
	// body of the controller. DELETE needs it whereas UPDATE does not because DELETE does NOT authorize inferred
	// types. (Thus forcing [FromBody] which does not call BindAsync)
	private static async Task<JsonHttpResult<ApiResponse>> Remove([FromServices] TService service,
		[FromServices] ILogService logService, HttpContext httpContext)
	{
		return await GenericController(async () =>
		{
			TDTO dto = await ReadWithBindAsync(httpContext);
			return await service.Remove(dto.ToModel());
		}, logService, httpContext);
	}

	#endregion

	// Use this method if custom binding is needed and you cannot have an inferred body parameter.
	private static async ValueTask<TDTO> ReadWithBindAsync(HttpContext httpContext)
	{
		// Verifies if the type has custom binding or not by using reflection
		bool hasCustomBinding = typeof(TDTO).GetInterfaces()
			.Any(c => c.IsGenericType && c.GetGenericTypeDefinition() == typeof(IExtensionBinder<>));
		if (!hasCustomBinding)
			return JsonSerializer.Deserialize<TDTO>(httpContext.Request.Body) ??
			       throw new ArgumentException("Empty body or malformed one");

		// Then it gets the BindAsync method through reflection.
		// Strongly inspired by:
		// https://stackoverflow.com/questions/74501978/how-do-i-test-if-a-type-t-implements-iparsablet
		MethodInfo? bind = typeof(TDTO).GetMethods(BindingFlags.Static | BindingFlags.Public).FirstOrDefault(c =>
			c.Name == "BindAsync" && c.GetParameters().Length == 1 &&
			c.GetParameters()[0].ParameterType == typeof(HttpContext));
		if (bind == null)
			throw new ArgumentException(
				"Bind: Trying to bind an IExtensionBinder value which does not have a bind method");

		// Invokes the function by casting its return type to an awaitable one.
		return await (ValueTask<TDTO?>)bind.Invoke(null, new object[] { httpContext })! ??
		       throw new ArgumentException("Empty body or binding failed");
	}

	#region Read

	private static async Task<JsonHttpResult<ApiResponse>> GetByID(TService service, ILogService logService,
		HttpContext httpContext, [FromRoute] int id)
	{
		return await GenericController(async () => await service.GetByID(id), logService,
			httpContext);
	}

	private static async Task<JsonHttpResult<ApiResponse>> GetByIDWithIncludes(TService service, ILogService logService,
		HttpContext httpContext, [FromRoute] int id, [FromBody] string[] includes)
	{
		return await GenericController(async () => await service.GetByID(id, includes: includes), logService,
			httpContext);
	}

	private static async Task<JsonHttpResult<ApiResponse>> GetAll(TService service, ILogService logService,
		HttpContext httpContext)
	{
		return await GenericController(async () => await service.GetAll(), logService,
			httpContext);
	}

	private static async Task<JsonHttpResult<ApiResponse>> GetAllWithIncludes(TService service, ILogService logService,
		HttpContext httpContext, [FromBody] string[] includes)
	{
		return await GenericController(async () => await service.GetAll(includes: includes), logService,
			httpContext);
	}

	private static async Task<JsonHttpResult<ApiResponse>> GetWithPagination(TService service, ILogService logService,
		HttpContext httpContext, [FromRoute] int nbItems, [FromRoute] int lastID,
		[FromBody] Pagination pagination)
	{
		return await GenericController(
			async () => await service.GetWithPagination(pagination, nbItems, lastID), logService,
			httpContext);
	}

	#endregion
}