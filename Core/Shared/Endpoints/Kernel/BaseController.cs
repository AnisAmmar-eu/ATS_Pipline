using Core.Shared.Models.ApiResponses;
using Core.Shared.Services.System.Logs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Core.Shared.Endpoints.Kernel;

public class BaseController
{
	protected static async Task<JsonHttpResult<ApiResponse>> GenericController<TReturn>(Func<Task<TReturn>> func,
		ILogService logService, HttpContext httpContext)
	{
		TReturn ans;
		try
		{
			ans = await func.Invoke();
		}
		catch (Exception e)
		{
			return await new ApiResponse().ErrorResult(logService, httpContext.GetEndpoint(), e);
		}

		return await new ApiResponse(ans).SuccessResult(logService, httpContext.GetEndpoint());
	}

	protected static async Task<JsonHttpResult<ApiResponse>> GenericControllerEmptyResponse(Func<Task> func,
		ILogService logService, HttpContext httpContext)
	{
		try
		{
			await func.Invoke();
		}
		catch (Exception e)
		{
			return await new ApiResponse().ErrorResult(logService, httpContext.GetEndpoint(), e);
		}

		return await new ApiResponse().SuccessResult(logService, httpContext.GetEndpoint());
	}
}