using Carter;
using Core.Entities.Packets.Services;
using Core.Shared.Models.ApiResponses;
using Core.Shared.Models.HttpResponse;
using Core.Shared.Services.System.Logs;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

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