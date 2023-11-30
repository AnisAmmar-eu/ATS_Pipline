using System.Collections.Specialized;
using System.Web;
using Core.Entities.User.Models.DTO.Acts.ActEntities;
using Core.Entities.User.Services.Acts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;

namespace Core.Shared.Authorize;

public class OldActAuthorizeLogic : IAuthorizationFilter
{
	private readonly IActService _actService;
	private readonly string? _entityProperty;
	private readonly string? _entityType;
	private readonly string? _parentProperty;
	private readonly string? _parentType;
	private readonly string _rid;

	public OldActAuthorizeLogic(
		IActService actService,
		string rid,
		string? entityType = null,
		string? entityProperty = null,
		string? parentType = null,
		string? parentProperty = null)
	{
		_rid = rid;
		_entityType = entityType;
		_entityProperty = entityProperty;
		_parentType = parentType;
		_parentProperty = parentProperty;
		_actService = actService;
	}

	public void OnAuthorization(AuthorizationFilterContext context)
	{
		if ((bool?)context.HttpContext.Items["IsActionTokenValid"] == true)
			return;

		RouteValueDictionary routeData = context.HttpContext.Request.RouteValues;
		NameValueCollection queryString
			= HttpUtility.ParseQueryString(context.HttpContext.Request.QueryString.ToString());

		int? entityID = null;
		int? parentID = null;

		if (_entityProperty is not null)
		{
			entityID = (int.TryParse((string?)routeData.GetValueOrDefault(_entityProperty), out int tmp)) ? tmp : null;
			entityID ??= (int.TryParse(queryString.Get(_entityProperty), out tmp)) ? tmp : null;
		}

		if (_parentProperty is not null)
		{
			parentID = (int.TryParse((string?)routeData.GetValueOrDefault(_parentProperty), out int tmp)) ? tmp : null;
			parentID ??= (int.TryParse(queryString.Get(_parentProperty), out tmp)) ? tmp : null;
		}

		bool result = _actService.ValidActionToken(
			context.HttpContext,
			new List<DTOActEntityToValid> { new() {
				Act = new() { RID = _rid, EntityType = _entityType, ParentType = _parentType },
				EntityID = entityID,
				ParentID = parentID, }, } );
		context.HttpContext.Items["IsActionTokenValid"] = result;
		if (!result)
            context.Result = new UnauthorizedResult();
    }
}