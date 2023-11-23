using Carter;
using Core.Shared.Models.ApiResponses;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ApiCameraAssign.Controllers;

public class CameraAssignController : ICarterModule
{
	public void AddRoutes(IEndpointRouteBuilder app)
	{
		app.MapGroup("apiCameraAssign").WithTags(nameof(CameraAssignController)).MapGet("status", GetStatus);
	}

	private static Ok<ApiResponse> GetStatus()
	{
		return new ApiResponse().SuccessResult();
	}
}