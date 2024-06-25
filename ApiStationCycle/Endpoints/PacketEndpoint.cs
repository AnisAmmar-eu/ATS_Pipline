using Carter;
using Core.Entities.Packets.Models.DB;
using Core.Entities.Packets.Models.DTO;
using Core.Entities.Packets.Services;
using Core.Shared.Dictionaries;
using Core.Shared.Endpoints.Kernel;
using Core.Shared.Endpoints.Kernel.Dictionaries;
using Core.Shared.Models.ApiResponses;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ApiStationCycle.Endpoints;

public class PacketEndpoint : BaseEntityEndpoint<Packet, DTOPacket, IPacketService>, ICarterModule
{
	public void AddRoutes(IEndpointRouteBuilder app)
	{
		RouteGroupBuilder group = app.MapGroup("apiStationCycle").WithTags(nameof(PacketEndpoint));
		group = MapBaseEndpoints(group, BaseEndpointFlags.Read);

		if (Station.IsServer)
			return;

		group.MapGet("mostRecent", GetMostRecent);
		group.MapGet("{shootingID}/{cameraID:int}/image", GetImageFromIDAndCamera);
		group.MapGet("oldest", GetOldest);
	}

	private static Task<JsonHttpResult<ApiResponse>> GetMostRecent(
		IPacketService packetService,
		HttpContext httpContext) => GenericEndpoint(packetService.GetMostRecentShooting, httpContext);

	private static Task<JsonHttpResult<ApiResponse>> GetOldest(
		IPacketService packetService,
		HttpContext httpContext) => GenericEndpoint(packetService.GetOldestNotSentTimestamp, httpContext);

	private static async Task<Results<FileContentHttpResult, JsonHttpResult<ApiResponse>>> GetImageFromIDAndCamera(
		int shootingID,
		int cameraID,
		IPacketService packetService,
		HttpContext httpContext)
	{
		byte[] image;
		DateTimeOffset ts;
		try
		{
			FileInfo imageFile = await packetService.GetImageFromCycleRIDAndCamera(shootingID, cameraID);
			ts = imageFile.CreationTime;
			image = await File.ReadAllBytesAsync(imageFile.FullName);
		}
		catch (Exception e)
		{
			return new ApiResponse().ErrorResult(httpContext, e);
		}

		httpContext.Response.Headers.Append("Access-Control-Expose-Headers", "Content-Disposition");
		return TypedResults.File(image, "image/jpeg", ts.ToUnixTimeMilliseconds().ToString());
	}
}