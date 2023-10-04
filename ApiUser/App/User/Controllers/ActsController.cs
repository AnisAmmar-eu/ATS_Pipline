﻿using Core.Entities.User.Dictionary;
using Core.Entities.User.Services.Acts;
using Core.Shared.Authorize;
using Core.Shared.Dictionary;
using Core.Shared.Models.HttpResponse;
using Core.Shared.Services.System.Logs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using Core.Entities.User.Models.DTO.Acts.ActEntities;

namespace ApiUser.App.User.Controllers
{
	/// <summary>
	/// Acts Routes
	/// </summary>
	[Route("apiUser/acts")]
	[ApiController]
	[Authorize]
	public class ActsController : ControllerBase
	{
		private readonly ILogsService _logsService;
		private readonly IActsService _actsService;
		/// <summary>
		/// Acts Constructor
		/// </summary>
		public ActsController(ILogsService logsService, IActsService actsService)
		{
			_logsService = logsService;
			_actsService = actsService;
		}

		// POST apiUser/acts/hasRights
		/// <summary>
		/// Check if a user has the rights to do an action
		/// </summary>
		/// <param name="dtoActEntityToValid"></param>
		/// <returns>The action token as string</returns>
		[HttpPost("hasRights")]
		public async Task<IActionResult> HasRights([FromBody] DTOActEntityToValid dtoActEntityToValid)
		{
			string actionToken;

			try
			{
				actionToken = await _actsService.HasRights(HttpContext, dtoActEntityToValid);
			}
			catch (Exception e)
			{
				return await new ApiResponseObject().ErrorResult(_logsService, ControllerContext, e);
			}
			return await new ApiResponseObject(200, new { actionToken }).SuccessResult(_logsService, ControllerContext);
		}

		// POST apiUser/acts/entity
		/// <summary>
		/// Get an actEntity with roles and users
		/// </summary>
		/// <param name="dtoActEntity"></param>
		/// <returns>A <see cref="DTOActEntity"/></returns>
		[HttpPost("entity")]
		public async Task<IActionResult> GetActEntityWithRoles([FromBody] DTOActEntity dtoActEntity)
		{
			try
			{
				dtoActEntity = await _actsService.GetActionEntityRoles(dtoActEntity);
			}
			catch (Exception e)
			{
				return await new ApiResponseObject().ErrorResult(_logsService, ControllerContext, e);
			}
			return await new ApiResponseObject(dtoActEntity).SuccessResult(_logsService, ControllerContext);
		}

		// POST apiUser/acts/list
		/// <summary>
		/// Retreive all actEntities status from a given list
		/// </summary>
		/// <param name="dtoActEntitiesStatus"></param>
		/// <returns>A <see cref="List{DTOActEntityStatus}"/></returns>
		[HttpPost("list")]
		public async Task<IActionResult> GetActEntitiesStatusFromList([FromBody] List<DTOActEntityStatus> dtoActEntitiesStatus)
		{
			try
			{
				dtoActEntitiesStatus = await _actsService.ActionsFromList(HttpContext, dtoActEntitiesStatus);
			}
			catch (Exception e)
			{
				return await new ApiResponseObject().ErrorResult(_logsService, ControllerContext, e);
			}
			return await new ApiResponseObject(dtoActEntitiesStatus).SuccessResult(_logsService, ControllerContext);
		}

		// PUT apiUser/acts/assign
		/// <summary>
		/// Assign acts from MANAGE
		/// </summary>
		/// <param name="dtoActEntities"></param>
		/// <returns></returns>
		/// <exception cref="UnauthorizedAccessException"></exception>
		[HttpPut("assign")]
		[ActAuthorize(ActionRID.ADMIN_GENERAL_RIGHTS)]
		public async Task<IActionResult> AssignActionsFromList([FromBody] List<DTOActEntity> dtoActEntities)
		{
			try
			{
				foreach (DTOActEntity dtoActEntity in dtoActEntities.Where(dto => dto.Act != null && dto.Act.RID.StartsWith("MANAGE.")))
					await _actsService.AssignAction(dtoActEntity);
			}
			catch (Exception e)
			{
				return await new ApiResponseObject().ErrorResult(_logsService, ControllerContext, e);
			}
			return await new ApiResponseObject().SuccessResult(_logsService, ControllerContext);
		}
	}
}
