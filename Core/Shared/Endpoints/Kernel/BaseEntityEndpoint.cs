using Core.Shared.Endpoints.Kernel.Dictionaries;
using Core.Shared.Models.ApiResponses;
using Core.Shared.Models.DB.Kernel.Interfaces;
using Core.Shared.Models.DTO.Kernel.Interfaces;
using Core.Shared.Paginations;
using Core.Shared.Paginations.Filtering;
using Core.Shared.Services.Kernel.Interfaces;
using Core.Shared.Services.SystemApp.Logs;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace Core.Shared.Endpoints.Kernel;

/// <summary>
/// Allows for an easy implementation of a generic CRUD.
/// </summary>
/// <typeparam name="T"></typeparam>
/// <typeparam name="TDTO"></typeparam>
/// <typeparam name="TService"></typeparam>
public class BaseEntityEndpoint<T, TDTO, TService> : BaseEndpoint
	where T : class, IBaseEntity<T, TDTO>
	where TDTO : class, IDTO<T, TDTO>
	where TService : IBaseEntityService<T, TDTO>
{
	private string[] _includes = Array.Empty<string>();
	private bool _isLogged;

	/// <summary>
	/// Calling this function creates the needed CRUD generic functions depending on which ones are asked for.
	/// </summary>
	/// <param name="group"></param>
	/// <param name="flags">
	/// <see cref="BaseEndpointFlags"/>specifying which endpoints are needed among Create, Read, Update and Delete.
	/// Allows to also disable logging
	/// </param>
	/// <param name="includes">List of includes to be in every request.</param>
	/// <returns></returns>
	protected RouteGroupBuilder MapBaseEndpoints(
		RouteGroupBuilder group,
		BaseEndpointFlags flags,
		params string[] includes)
	{
		string dtoName = typeof(TDTO).Name;
		string tName = typeof(T).Name;
		_includes = includes;
		_isLogged = flags.HasFlag(BaseEndpointFlags.ToLogs);
		group = group.MapGroup(tName);

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

			group.MapGet("{columnName}/{filterValue}", GetByGeneric)
				.WithSummary($"Get a {tName} by a filterValue in columnName")
				.WithOpenApi();
			group.MapPut("{columnName}/{filterValue}", GetByGenericWithIncludes)
				.WithSummary($"Get a {tName} by a filterValue in columnName with includes")
				.WithOpenApi();

			group.MapPut("pagination", CountWithPagination)
				.WithSummary($"Get the number of {tName}s available in the filter and search with includes required")
				.WithOpenApi();
			group.MapPut("pagination/{nbItems}", GetWithPagination)
				.WithSummary($"Get {tName}s by paging, sorting, text search and filtering with includes")
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
			return group;

		group.MapDelete("{id}", Remove)
			.WithSummary($"Remove the {dtoName} by its ID")
			.WithOpenApi();

		return group;
	}

	#region Create

	private Task<JsonHttpResult<ApiResponse>> Add(
		TService service,
		ILogService logService,
		HttpContext httpContext,
		TDTO dto)
	{
		return GenericEndpoint(() => service.Add(dto.ToModel()), logService, httpContext, _isLogged);
	}

	#endregion

	#region Read

	private Task<JsonHttpResult<ApiResponse>> GetAll(
		TService service,
		ILogService logService,
		HttpContext httpContext)
	{
		return GenericEndpoint(
			() => service.GetAll(withTracking: false, includes: _includes),
			logService,
			httpContext,
			_isLogged);
	}

	private Task<JsonHttpResult<ApiResponse>> GetAllWithIncludes(
		TService service,
		ILogService logService,
		HttpContext httpContext,
		[FromBody] string[] includes)
	{
		return GenericEndpoint(
			() => service.GetAll(withTracking: false, includes: includes),
			logService,
			httpContext,
			_isLogged);
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
				return service.GetWithPagination(pagination, 0);
			},
			logService,
			httpContext,
			_isLogged);
	}

	private Task<JsonHttpResult<ApiResponse>> GetByGenericWithIncludes(
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
				return service.GetWithPagination(pagination, 0);
			},
			logService,
			httpContext,
			_isLogged);
	}

	private Task<JsonHttpResult<ApiResponse>> GetWithPagination(
		TService service,
		ILogService logService,
		HttpContext httpContext,
		[FromRoute] int nbItems,
		[FromBody] Pagination pagination)
	{
		return GenericEndpoint( () => service.GetWithPagination(pagination, nbItems), logService, httpContext, _isLogged);
	}

	private Task<JsonHttpResult<ApiResponse>> CountWithPagination(
		TService service,
		ILogService logService,
		HttpContext httpContext,
		[FromBody] Pagination pagination)
	{
		return GenericEndpoint( () => service.CountWithPagination(pagination), logService, httpContext, _isLogged);
	}

	#endregion

	#region Update

	private Task<JsonHttpResult<ApiResponse>> Update(
		TService service,
		ILogService logService,
		HttpContext httpContext,
		TDTO dto)
	{
		return GenericEndpoint(() => service.Update(dto.ToModel()), logService, httpContext, _isLogged);
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
			httpContext,
			_isLogged);
	}

	#endregion
}