using System.ComponentModel.DataAnnotations;
using Core.Entities.User.Models.DTO.Roles;
using Core.Entities.User.Services.Roles;
using Core.Shared.Models.HttpResponse;
using Core.Shared.Services.System.Logs;
using Microsoft.AspNetCore.Mvc;

namespace ApiUser.App.User.Controllers;

/// <summary>
///     Roles Routes
/// </summary>
[Route("apiUser/roles")]
[ApiController]
public class RolesController : ControllerBase
{
	private readonly ILogService _logService;
	private readonly IRolesService _rolesService;

	/// <summary>
	///     Roles Constructor
	/// </summary>
	/// <param name="logService"></param>
	/// <param name="rolesService"></param>
	public RolesController(ILogService logService, IRolesService rolesService)
	{
		_logService = logService;
		_rolesService = rolesService;
	}

	// GET apiUser/roles
	/// <summary>
	///     Get all roles
	/// </summary>
	/// <returns>A <see cref="List{DTORole}" /></returns>
	[HttpGet]
	public async Task<IActionResult> GetAll()
	{
		List<DTORole> dtoRoles;
		try
		{
			dtoRoles = await _rolesService.GetAll();
		}
		catch (Exception e)
		{
			return await new ApiResponseObject().ErrorResult(_logService, ControllerContext, e);
		}

		return await new ApiResponseObject(dtoRoles).SuccessResult(_logService, ControllerContext);
	}

	// GET apiUser/roles/{rid}
	/// <summary>
	///     Get a role by RID
	/// </summary>
	/// <param name="rid"></param>
	/// <returns>The selected <see cref="DTORole" /></returns>
	[HttpGet("{rid}")]
	public async Task<IActionResult> GetByRID([Required] string rid)
	{
		DTORole dtoRole;
		try
		{
			dtoRole = await _rolesService.GetByRID(rid);
		}
		catch (Exception e)
		{
			return await new ApiResponseObject().ErrorResult(_logService, ControllerContext, e);
		}

		return await new ApiResponseObject(dtoRole).SuccessResult(_logService, ControllerContext);
	}

	// POST apiUser/roles
	/// <summary>
	///     Create a new role
	/// </summary>
	/// <param name="dtoRole"></param>
	/// <returns>The create <see cref="DTORole" /></returns>
	[HttpPost]
	public async Task<IActionResult> Create([FromBody] DTORole dtoRole)
	{
		try
		{
			dtoRole = await _rolesService.Create(dtoRole);
		}
		catch (Exception e)
		{
			return await new ApiResponseObject().ErrorResult(_logService, ControllerContext, e);
		}

		return await new ApiResponseObject(dtoRole).SuccessResult(_logService, ControllerContext);
	}

	// PUT apiUser/roles/{rid}
	/// <summary>
	///     Update a role by RID
	/// </summary>
	/// <param name="rid"></param>
	/// <param name="dtoRole"></param>
	/// <returns>The updated <see cref="DTORole" /></returns>
	[HttpPut("{rid}")]
	public async Task<IActionResult> Update([Required] string rid, [FromBody] DTORole dtoRole)
	{
		try
		{
			dtoRole = await _rolesService.Update(rid, dtoRole);
		}
		catch (Exception e)
		{
			return await new ApiResponseObject().ErrorResult(_logService, ControllerContext, e);
		}

		return await new ApiResponseObject(dtoRole).SuccessResult(_logService, ControllerContext);
	}

	// DELETE apiUser/roles/{rid}
	/// <summary>
	///     Delete a role by RID
	/// </summary>
	/// <param name="rid"></param>
	/// <returns>A string</returns>
	[HttpDelete("{rid}")]
	public async Task<IActionResult> Delete([Required] string rid)
	{
		try
		{
			await _rolesService.Delete(rid);
		}
		catch (Exception e)
		{
			return await new ApiResponseObject().ErrorResult(_logService, ControllerContext, e);
		}

		return await new ApiResponseObject("The role with RID {" + rid + "} has been successfully deleted.")
			.SuccessResult(_logService, ControllerContext);
	}
}