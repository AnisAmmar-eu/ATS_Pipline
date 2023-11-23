﻿using System.ComponentModel.DataAnnotations;
using Carter;
using Core.Entities.User.Models.DTO.Roles;
using Core.Entities.User.Services.Roles;
using Core.Shared.Endpoints.Kernel;
using Core.Shared.Models.ApiResponses;
using Core.Shared.Models.DB.Kernel;
using Core.Shared.Models.DTO.Kernel;
using Core.Shared.Services.Kernel.Interfaces;
using Core.Shared.Services.System.Logs;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ApiUser.App.User.Controllers;

/// <summary>
///     Roles Routes
/// </summary>
public class RoleController : BaseController, ICarterModule
{
	/// <summary>
	///	Add Routes from CarterModule
	/// </summary>
	/// <param name="app"></param>
	/// <exception cref="NotImplementedException"></exception>
	public void AddRoutes(IEndpointRouteBuilder app)
	{
		RouteGroupBuilder group = app.MapGroup("apiUser/roles").WithTags(nameof(RoleController));

		group.MapGet("", GetAll);
		group.MapGet("{rid}", GetByRID);
		group.MapPost("", Create);
		group.MapPut("{rid}", Update);
		group.MapDelete("{rid}", Delete);
	}

	// GET apiUser/roles
	/// <summary>
	///     Get all roles
	/// </summary>
	/// <returns>A <see cref="List{DTORole}" /></returns>
	private static async Task<JsonHttpResult<ApiResponse>> GetAll(IRoleService roleService, ILogService logService,
		HttpContext httpContext)
	{
		return await GenericController(async () => await roleService.GetAll(), logService, httpContext);
	}

	// GET apiUser/roles/{rid}
	/// <summary>
	///     Get a role by RID
	/// </summary>
	/// <param name="rid"></param>
	/// <param name="roleService"></param>
	/// <param name="logService"></param>
	/// <param name="httpContext"></param>
	/// <returns>The selected <see cref="DTORole" /></returns>
	private static async Task<JsonHttpResult<ApiResponse>> GetByRID([Required] string rid, IRoleService roleService,
		ILogService logService, HttpContext httpContext)
	{
		return await GenericController(async () => await roleService.GetByRID(rid), logService, httpContext);
	}

	// POST apiUser/roles
	/// <summary>
	///     Create a new role
	/// </summary>
	/// <param name="dtoRole"></param>
	/// <param name="roleService"></param>
	/// <param name="logService"></param>
	/// <param name="httpContext"></param>
	/// <returns>The create <see cref="DTORole" /></returns>
	private static async Task<JsonHttpResult<ApiResponse>> Create([FromBody] DTORole dtoRole, IRoleService roleService,
		ILogService logService, HttpContext httpContext)
	{
		return await GenericController(async () => await roleService.Create(dtoRole), logService, httpContext);
	}

	// PUT apiUser/roles/{rid}
	/// <summary>
	///     Update a role by RID
	/// </summary>
	/// <param name="rid"></param>
	/// <param name="dtoRole"></param>
	/// <param name="roleService"></param>
	/// <param name="logService"></param>
	/// <param name="httpContext"></param>
	/// <returns>The updated <see cref="DTORole" /></returns>
	private static async Task<JsonHttpResult<ApiResponse>> Update([Required] string rid, [FromBody] DTORole dtoRole,
		IRoleService roleService, ILogService logService, HttpContext httpContext)
	{
		return await GenericController(async () => await roleService.Update(rid, dtoRole), logService, httpContext);
	}

	// DELETE apiUser/roles/{rid}
	/// <summary>
	///     Delete a role by RID
	/// </summary>
	/// <param name="rid"></param>
	/// <param name="roleService"></param>
	/// <param name="logService"></param>
	/// <param name="httpContext"></param>
	/// <returns>A string</returns>
	private static async Task<JsonHttpResult<ApiResponse>> Delete([Required] string rid, IRoleService roleService,
		ILogService logService, HttpContext httpContext)
	{
		return await GenericController(async () =>
		{
			await roleService.Delete(rid);
			return $"The role with RID {{{rid}}} has been successfully deleted.";
		}, logService, httpContext);
	}
}