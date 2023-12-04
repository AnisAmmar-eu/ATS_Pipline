using System.Diagnostics;
using Core.Shared.Models.ApiResponses;
using Core.Shared.Services.System.Logs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Core.Shared.Endpoints.Kernel;

public class BaseEndpoint
{
	protected static async Task<JsonHttpResult<ApiResponse>> GenericEndpoint<TReturn>(
		Func<Task<TReturn>> func,
		ILogService logService,
		HttpContext httpContext,
		bool isLogged = true)
	{
		TReturn ans;
		List<object> a = [];
		Stopwatch watch = new();
		try
		{
			ans = await func.Invoke();
			for (int i = 0; i < 12; ++i)
			{
				watch.Restart();
				await func.Invoke();
				watch.Stop();
				a.Add(watch.Elapsed);
			}
		}
		catch (Exception e)
		{
			return await new ApiResponse().ErrorResult(logService, httpContext, e);
		}

		a.Add(ans);

		if (isLogged)
			return await new ApiResponse(a).SuccessResult(logService, httpContext);

		return new ApiResponse(a).SuccessResult();
	}

	protected static async Task<JsonHttpResult<ApiResponse>> GenericEndpointEmptyResponse(
		Func<Task> func,
		ILogService logService,
		HttpContext httpContext,
		bool isLogged = true)
	{
		try
		{
			await func.Invoke();
		}
		catch (Exception e)
		{
			return await new ApiResponse().ErrorResult(logService, httpContext, e);
		}

		if (isLogged)
			return await new ApiResponse().SuccessResult(logService, httpContext);

		return new ApiResponse().SuccessResult();
	}
}