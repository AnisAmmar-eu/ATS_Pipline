using Core.Shared.Models.ApiResponses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Core.Shared.Endpoints.Kernel;

/// <summary>
/// Provides a set of generic endpoints. These one encapsulate a function in a try catch block which return a correct
/// <see cref="ApiResponse"/> in case of failure or success.
/// </summary>
public class BaseEndpoint
{
	protected static async Task<JsonHttpResult<ApiResponse>> GenericEndpoint<TReturn>(
		Func<Task<TReturn>> func,
		HttpContext httpContext,
		bool isLogged = false)
	{
		TReturn ans;
		try
		{
			ans = await func.Invoke();
		}
		catch (Exception e)
		{
			return new ApiResponse().ErrorResult(httpContext, e);
		}

		return isLogged
			? new ApiResponse(ans).SuccessResult(httpContext)
			: new ApiResponse(ans).SuccessResult();
	}

	protected static async Task<JsonHttpResult<ApiResponse>> GenericEndpointEmptyResponse(
		Func<Task> func,
		HttpContext httpContext,
		bool isLogged = false)
	{
		try
		{
			await func.Invoke();
		}
		catch (Exception e)
		{
			return new ApiResponse().ErrorResult(httpContext, e);
		}

		return isLogged ? new ApiResponse().SuccessResult(httpContext) : new ApiResponse().SuccessResult();
	}
}