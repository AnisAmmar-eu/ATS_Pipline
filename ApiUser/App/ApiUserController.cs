using Carter;
using Core.Shared.Models.ApiResponses;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ApiUser.App;

/// <summary>
///     Api for general API operations eg. get status.
/// </summary>
public class ApiUserController : ICarterModule
{
	/// <summary>
	///     Add Routes from CarterModule
	/// </summary>
	/// <param name="app"></param>
	public void AddRoutes(IEndpointRouteBuilder app)
	{
		app.MapGroup("apiUser").WithTags(nameof(ApiUserController)).MapGet("status", GetStatus);
	}

	/// <summary>
	///     Returns 200. Useful to know if the API is down or not.
	/// </summary>
	/// <returns></returns>
	private static JsonHttpResult<ApiResponse> GetStatus()
	{
		return new ApiResponse().SuccessResult();
	}
}