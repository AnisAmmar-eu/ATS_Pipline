using Carter;
using Core.Shared.Models.ApiResponses;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ApiUser.Endpoints;

/// <summary>
///     Api for general API operations eg. get status.
/// </summary>
public class ApiUserEndpoint : ICarterModule
{
	/// <summary>
	///     Add Routes from CarterModule
	/// </summary>
	/// <param name="app"></param>
	public void AddRoutes(IEndpointRouteBuilder app)
		=> app.MapGroup("apiUser").WithTags(nameof(ApiUserEndpoint))
			.MapGet("status", () => new ApiResponse().SuccessResult());
}