using System.Collections.Specialized;
using System.Net;
using System.Web;
using Core.Entities.User.Models.DTO.Acts;
using Core.Entities.User.Models.DTO.Acts.ActEntities;
using Core.Entities.User.Services.Acts;
using Core.Shared.Models.ApiResponses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace Core.Shared.Authorize;

public class ActAuthorizeHandler : AuthorizationHandler<ActAuthorize>
{
	private readonly IHttpContextAccessor _httpContextAccessor;
	private readonly IActService _actService;
	
	public ActAuthorizeHandler(IHttpContextAccessor httpContextAccessor, IActService actService)
	{
		_httpContextAccessor = httpContextAccessor;
		_actService = actService;
	}

	protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ActAuthorize requirement)
	{
		HttpContext? httpContext = _httpContextAccessor.HttpContext;
		if (httpContext == null)
		{
			context.Fail(new AuthorizationFailureReason(this, "No httpContext"));
			return Task.CompletedTask;
		}

		bool result = _actService.ValidActionToken(httpContext,
			new List<DTOActEntityToValid>
			{
				new()
				{
					Act = new DTOAct
					{
						RID = requirement.RID,
					},
					EntityID = null,
					ParentID = null
				}
			}
		);
		if (!result)
		{
			context.Fail(new AuthorizationFailureReason(this, "Access denied"));
		}
		else context.Succeed(requirement);

		return Task.CompletedTask;
	}
}