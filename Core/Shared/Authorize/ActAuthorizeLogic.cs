using Core.Entities.User.Services.Acts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Web;
using Core.Entities.User.Models.DTO.Acts.ActEntities;

namespace Core.Shared.Authorize
{
	public class ActAuthorizeLogic : IAuthorizationFilter
	{
		private readonly string _rid;
		private readonly string? _parentType;
		private readonly string? _parentProperty;
		private readonly string? _entityType;
		private readonly string? _entityProperty;
		private readonly IActsService _actsService;
		public ActAuthorizeLogic(IActsService actsService, string rid, string? entityType = null, string? entityProperty = null, string? parentType = null, string? parentProperty = null)
		{
			_rid = rid;
			_entityType = entityType;
			_entityProperty = entityProperty;
			_parentType = parentType;
			_parentProperty = parentProperty;
			_actsService = actsService;
		}

		public void OnAuthorization(AuthorizationFilterContext context)
		{
			if ((bool?)context.HttpContext.Items["IsActionTokenValid"] == true)
				return;

			var routeData = context.HttpContext.Request.RouteValues;
			var queryString = HttpUtility.ParseQueryString(context.HttpContext.Request.QueryString.ToString());

			int? entityID = null;
			int? parentID = null;

			if (_entityProperty != null)
			{
				entityID = int.TryParse((string?)routeData.GetValueOrDefault(_entityProperty), out int tmp) ? tmp : null;
				entityID ??= int.TryParse(queryString.Get(_entityProperty), out tmp) ? tmp : null;
			}
			if (_parentProperty != null)
			{
				parentID = int.TryParse((string?)routeData.GetValueOrDefault(_parentProperty), out int tmp) ? tmp : null;
				parentID ??= int.TryParse(queryString.Get(_parentProperty), out tmp) ? tmp : null;
			}

			bool result = _actsService.ValidActionToken(context.HttpContext,
				 new List<DTOActEntityToValid>
					{
						new()
						{
							Act = new()
							{
								RID = _rid,
								EntityType = _entityType,
								ParentType = _parentType
							},
							EntityID = entityID,
							ParentID = parentID
						}
					}
			);
			context.HttpContext.Items["IsActionTokenValid"] = result;
			if (!result)
			{
				context.Result = new UnauthorizedResult();
			}
		}
	}
}
