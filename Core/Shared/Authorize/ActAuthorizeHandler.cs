using Core.Entities.User.Models.DTO.Acts.ActEntities;
using Core.Entities.User.Services.Acts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace Core.Shared.Authorize;

public class ActAuthorizeHandler : AuthorizationHandler<ActAuthorize>
{
	private readonly IActService _actService;
	private readonly IHttpContextAccessor _httpContextAccessor;

	public ActAuthorizeHandler(IHttpContextAccessor httpContextAccessor, IActService actService)
	{
		_httpContextAccessor = httpContextAccessor;
		_actService = actService;
	}

	protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ActAuthorize requirement)
	{
		HttpContext? httpContext = _httpContextAccessor.HttpContext;
		if (httpContext is null)
		{
			context.Fail(new AuthorizationFailureReason(this, "No httpContext"));
			return Task.CompletedTask;
		}

		bool result = _actService.ValidActionToken(
			httpContext,
			new List<DTOActEntityToValid> {
				new() { Act = new() { RID = requirement.RID }, EntityID = null, ParentID = null } } );
		if (!result)
			context.Fail(new AuthorizationFailureReason(this, "Access denied"));
		else
            context.Succeed(requirement);

        return Task.CompletedTask;
	}
}