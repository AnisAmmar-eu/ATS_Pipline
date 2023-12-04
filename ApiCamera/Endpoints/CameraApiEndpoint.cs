using ApiCamera.Utils;
using Carter;
using Core.Entities.IOT.IOTTags.Services;
using Core.Entities.Packets.Dictionaries;
using Core.Shared.Dictionaries;
using Core.Shared.Endpoints.Kernel;
using Core.Shared.Models.ApiResponses;
using Core.Shared.Models.Camera;
using Core.Shared.Services.System.Logs;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Stemmer.Cvb;

namespace ApiCamera.Endpoints;

public class CameraApiEndpoint : BaseEndpoint, ICarterModule
{
	public void AddRoutes(IEndpointRouteBuilder app)
	{
		RouteGroupBuilder group = app.MapGroup("apiCamera").WithTags(nameof(CameraApiEndpoint));

		group.MapGet("status", GetStatus);
		group.MapGet("acquisition", AcquisitionAsync);
		group.MapGet("{cameraID}/testImage", GetCameraTestImage);
	}

	private static JsonHttpResult<ApiResponse> GetStatus()
	{
		return new ApiResponse().SuccessResult();
	}

	#region Acquisition

	private static Task<JsonHttpResult<ApiResponse>> AcquisitionAsync(
		IConfiguration configuration,
		IIOTTagService iotTagService,
		ILogService logService,
		HttpContext httpContext)
	{
		return GenericEndpointEmptyResponse(
			async () =>
			{
				int port1 = configuration.GetValue<int>("CameraConfig:Camera1:Port");
				int port2 = configuration.GetValue<int>("CameraConfig:Camera2:Port");
				Device device1 = CameraConnectionManager.Connect(port1);
				if (Station.Type != StationType.S5)
				{
					// Create an instance of the camera
					Device device2 = CameraConnectionManager.Connect(port2);
					Task task1 = CameraUtils.RunAcquisition(
						iotTagService,
						device1,
						"jpg",
						ShootingUtils.Camera1,
						ShootingUtils.CameraTest1);
					Task task2 = CameraUtils.RunAcquisition(
						iotTagService,
						device2,
						"jpg",
						ShootingUtils.Camera2,
						ShootingUtils.CameraTest2);
					await task1;
					await task2;
				}
				else
				{
					await CameraUtils.RunAcquisition(
						iotTagService,
						device1,
						"jpg",
						ShootingUtils.Camera1,
						ShootingUtils.CameraTest1,
						ShootingUtils.Camera2,
						ShootingUtils.CameraTest2);
				}
			},
			logService,
			httpContext);
	}

	#endregion

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

	#endregion
}