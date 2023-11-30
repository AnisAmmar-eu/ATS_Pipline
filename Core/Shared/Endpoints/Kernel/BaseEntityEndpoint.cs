using System.Reflection;
using System.Text.Json;
using Core.Shared.Endpoints.Kernel.Dictionaries;
using Core.Shared.Models.ApiResponses;
using Core.Shared.Models.DB.Kernel.Interfaces;
using Core.Shared.Models.DTO.Kernel.Interfaces;
using Core.Shared.Paginations;
using Core.Shared.Paginations.Filtering;
using Core.Shared.Services.Kernel.Interfaces;
using Core.Shared.Services.System.Logs;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace Core.Shared.Endpoints.Kernel;

public class BaseEntityEndpoint<T, TDTO, TService> : BaseEndpoint
	where T : class, IBaseEntity<T, TDTO>
	where TDTO : class, IDTO<T, TDTO>
	where TService : IBaseEntityService<T, TDTO>
{
	private string[] _includes = Array.Empty<string>();

	protected void MapBaseEndpoints(RouteGroupBuilder group, BaseEndpointFlags flags, params string[] includes)
	{
		string dtoName = typeof(TDTO).Name;
		string tName = typeof(T).Name;
		_includes = includes;
		if (flags.HasFlag(BaseEndpointFlags.Create))
			group.MapPost(string.Empty, Add).WithSummary($"Add the {dtoName} in the body to the database") .WithOpenApi();

		if (flags.HasFlag(BaseEndpointFlags.Read))
		{
			group.MapGet(string.Empty, GetAll)
				.WithSummary($"Get all {tName}s")
				.WithOpenApi();
			group.MapPut(string.Empty, GetAllWithIncludes)
				.WithSummary($"Get all {tName}s with includes listed in body")
				.WithOpenApi();

			group.MapGet("id/{id}", GetByID)
				.WithSummary($"Get a {tName} by ID")
				.WithOpenApi();
			group.MapPut("id/{id}", GetByIDWithIncludes)
				.WithSummary($"Get all {tName}s by ID with includes listed in body")
				.WithOpenApi();

			group.MapGet("{columnName}/{filterValue}", GetByGeneric)
				.WithSummary($"Get a {tName} by a filterValue in columnName")
				.WithOpenApi();
			group.MapPut("{columnName}/{filterValue}", GetByGenericWithIncludes)
				.WithSummary($"Get a {tName} by a filterValue in columnName with includes")
				.WithOpenApi();

			group.MapPut("pagination/{nbItems}/{lastID}", GetWithPagination)
				.WithSummary($"Get a {tName} by paging, sorting and filtering with includes")
				.WithOpenApi();
		}

		if (flags.HasFlag(BaseEndpointFlags.Update))
        {
            group.MapPut("update", Update)
                .WithSummary($"Update the {dtoName} in the body if it already exist")
                .Accepts<TDTO>("application/json")
                .WithOpenApi();
        }

        if (!flags.HasFlag(BaseEndpointFlags.Delete))
			return;

		group.MapDelete("{id}", Remove)
			.WithSummary($"Remove the {dtoName} by its ID")
			.WithOpenApi();
	}

	#region Create

	private static Task<JsonHttpResult<ApiResponse>> Add(
		TService service,
		ILogService logService,
		HttpContext httpContext,
		TDTO dto)
	{
		return GenericEndpoint(() => service.Add(dto.ToModel()), logService, httpContext);
	}

	#endregion

	#region Update

	private static Task<JsonHttpResult<ApiResponse>> Update(
		TService service,
		ILogService logService,
		HttpContext httpContext,
		TDTO dto)
	{
		return GenericEndpoint(() => service.Update(dto.ToModel()), logService, httpContext);
	}

	#endregion

	#region Delete

	private Task<JsonHttpResult<ApiResponse>> Remove(
		[FromServices] TService service,
		[FromServices] ILogService logService,
		HttpContext httpContext,
		[FromRoute] int id)
	{
		return GenericEndpointEmptyResponse(
			() => service.Remove(id, _includes),
			logService,
			httpContext);
	}

	#endregion

	/// <summary>
	///     Use this method if custom binding is needed and you cannot have an inferred body parameter.
	///     Now unused but left there in case.
	/// </summary>
	/// <param name="httpContext"></param>
	private static async ValueTask<TDTO> ReadWithBindAsync(HttpContext httpContext)
	{
		// Verifies if the type has custom binding or not by using reflection
		bool hasCustomBinding = typeof(TDTO).GetInterfaces()
			.Any(c => c.IsGenericType && c.GetGenericTypeDefinition() == typeof(IExtensionBinder<>));
		if (!hasCustomBinding)
        {
            return JsonSerializer.Deserialize<TDTO>(httpContext.Request.Body)
                ?? throw new ArgumentException("Empty body or malformed one");
        }

        // Then it gets the BindAsync method through reflection.
        // Strongly inspired by:
        // https://stackoverflow.com/questions/74501978/how-do-i-test-if-a-type-t-implements-iparsablet
        MethodInfo? bind = Array.Find(
               typeof(TDTO).GetMethods(BindingFlags.Static | BindingFlags.Public),
               c =>
                      c.Name == "BindAsync"
                             && c.GetParameters().Length == 1
                             && c.GetParameters()[0].ParameterType == typeof(HttpContext));
		if (bind is null)
        {
            throw new ArgumentException(
                "Bind: Trying to bind an IExtensionBinder value which does not have a bind method");
        }

        // Invokes the function by casting its return type to an awaitable one.
        return await (ValueTask<TDTO?>)bind.Invoke(null, new object[] { httpContext })! ??
               throw new ArgumentException("Empty body or binding failed");
	}

	#region Read

	private Task<JsonHttpResult<ApiResponse>> GetAll(
		TService service,
		ILogService logService,
		HttpContext httpContext)
	{
		return GenericEndpoint(
			() => service.GetAll(withTracking: false, includes: _includes),
			logService,
			httpContext);
	}

	private static Task<JsonHttpResult<ApiResponse>> GetAllWithIncludes(
		TService service,
		ILogService logService,
		HttpContext httpContext,
		[FromBody] string[] includes)
	{
		return GenericEndpoint(
			() => service.GetAll(withTracking: false, includes: includes),
			logService,
			httpContext);
	}

	private Task<JsonHttpResult<ApiResponse>> GetByID(
		TService service,
		ILogService logService,
		HttpContext httpContext,
		[FromRoute] int id)
	{
		return GenericEndpoint(
			() => service.GetByID(id, withTracking: false, includes: _includes),
			logService,
			httpContext);
	}

	private static Task<JsonHttpResult<ApiResponse>> GetByIDWithIncludes(
		TService service,
		ILogService logService,
		HttpContext httpContext,
		[FromRoute] int id,
		[FromBody] string[] includes)
	{
		return GenericEndpoint(
			() => service.GetByID(id, withTracking: false, includes: includes),
			logService,
			httpContext);
	}

	private Task<JsonHttpResult<ApiResponse>> GetByGeneric(
		TService service,
		ILogService logService,
		HttpContext httpContext,
		[FromRoute] string columnName,
		[FromRoute] string filterValue)
	{
		return GenericEndpoint(
			() =>
			{
				FilterParam param = new() {
					ColumnName = columnName,
					FilterValue = filterValue,
					FilterOptionName = "Equal",
				};
				Pagination pagination = new() {
					Includes = _includes,
					FilterParams = [param],
				};
				return service.GetWithPagination(pagination, 0, 0);
			},
			logService,
			httpContext);
	}

	private static Task<JsonHttpResult<ApiResponse>> GetByGenericWithIncludes(
		TService service,
		ILogService logService,
		HttpContext httpContext,
		[FromRoute] string columnName,
		[FromRoute] string filterValue,
		[FromBody] string[] includes)
	{
		return GenericEndpoint(
			() =>
			{
				FilterParam param = new() {
					ColumnName = columnName,
					FilterValue = filterValue,
					FilterOptionName = "Equal",
				};
				Pagination pagination = new() {
					Includes = includes,
					FilterParams = [param],
				};
				return service.GetWithPagination(pagination, 0, 0);
			},
			logService,
			httpContext);
	}

	private static Task<JsonHttpResult<ApiResponse>> GetWithPagination(
		TService service,
		ILogService logService,
		HttpContext httpContext,
		[FromRoute] int nbItems,
		[FromRoute] int lastID,
		[FromBody] Pagination pagination)
	{
		return GenericEndpoint(
			() => service.GetWithPagination(pagination, nbItems, lastID),
			logService,
			httpContext);
	}

	#endregion
}