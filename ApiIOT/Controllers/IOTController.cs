using Carter;
using Core.Shared.Models.ApiResponses;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ApiIOT.Controllers;

public class IOTController : ICarterModule
{
	public void AddRoutes(IEndpointRouteBuilder app)
	{
		app.MapGroup("apiIOT").WithTags(nameof(IOTController)).MapGet("status", GetStatus);
	}

	private static Ok<ApiResponse> GetStatus()
	{
		return new ApiResponse().SuccessResult();
	}
}