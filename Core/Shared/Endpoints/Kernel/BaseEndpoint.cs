using Core.Shared.Models.ApiResponses;
using Core.Shared.Services.System.Logs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Core.Shared.Endpoints.Kernel;

public class BaseEndpoint
{
	protected static async Task<JsonHttpResult<ApiResponse>> GenericEndpoint<TReturn>(Func<Task<TReturn>> func,
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

	protected static async Task<JsonHttpResult<ApiResponse>> GenericEndpointEmptyResponse(Func<Task> func,
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