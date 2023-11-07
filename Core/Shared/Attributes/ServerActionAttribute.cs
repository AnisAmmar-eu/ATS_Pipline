using Core.Shared.Dictionaries;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Core.Shared.Attributes;

public class ServerActionAttribute : ActionFilterAttribute
{
	public override void OnActionExecuting(ActionExecutingContext context)
	{
		if (!Station.IsServer)
			context.Result = new NotFoundResult();
		else
			base.OnActionExecuting(context);
	}
}