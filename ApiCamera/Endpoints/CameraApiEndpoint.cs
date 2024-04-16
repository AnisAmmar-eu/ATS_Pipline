using Carter;
using Core.Entities.Packets.Dictionaries;
using Core.Shared.Endpoints.Kernel;
using Core.Shared.Models.ApiResponses;
using Core.Shared.Services.SystemApp.Logs;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ApiCamera.Endpoints;

public class CameraApiEndpoint : BaseEndpoint, ICarterModule
{
	public void AddRoutes(IEndpointRouteBuilder app)
	{
		RouteGroupBuilder group = app.MapGroup("apiCamera").WithTags(nameof(CameraApiEndpoint));

		group.MapGet("status", () => new ApiResponse().SuccessResult());
		group.MapGet("{cameraID:int}/testImage", GetCameraTestImage);
	}

	#region Get TestImages

	private static async Task<Results<FileContentHttpResult, JsonHttpResult<ApiResponse>>> GetCameraTestImage(
		[FromRoute] int cameraID,
		ILogService logService,
		HttpContext httpContext)
	{
		byte[] image;
		DateTimeOffset ts;
		try
		{
			string cameraTestPath = (cameraID == 1) ? ShootingUtils.CameraTest1 : ShootingUtils.CameraTest2;
			FileInfo imageFile = new(cameraTestPath + ShootingUtils.TestFilename);
			ts = imageFile.CreationTime;
			image = await File.ReadAllBytesAsync(imageFile.FullName);
		}
		catch (Exception e)
		{
			return await new ApiResponse().ErrorResult(logService, httpContext, e);
		}

		httpContext.Response.Headers.Append("Access-Control-Expose-Headers", "Content-Disposition");
		return TypedResults.File(image, "image/jpeg", ts.ToUnixTimeMilliseconds().ToString());
	}

	#endregion Get TestImages
}