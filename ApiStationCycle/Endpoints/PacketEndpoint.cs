using Carter;
using Core.Entities.Packets.Models.DB;
using Core.Entities.Packets.Models.DTO;
using Core.Entities.Packets.Services;
using Core.Shared.Dictionaries;
using Core.Shared.Endpoints.Kernel;
using Core.Shared.Endpoints.Kernel.Dictionaries;
using Core.Shared.Models.ApiResponses;
using Core.Shared.Services.SystemApp.Logs;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ApiStationCycle.Endpoints;

public class PacketEndpoint : BaseEntityEndpoint<Packet, DTOPacket, IPacketService>, ICarterModule
{
	public void AddRoutes(IEndpointRouteBuilder app)
	{
		if (Station.IsServer)
			return;

		RouteGroupBuilder group = app.MapGroup("apiStationCycle").WithTags(nameof(PacketEndpoint));
		group = MapBaseEndpoints(group, BaseEndpointFlags.Read);

		group.MapGet("mostRecent", GetMostRecent);
		group.MapGet("{shootingID}/{cameraID:int}/image", GetImageFromIDAndCamera);
		group.MapGet("oldest", GetOldest);
	}

	private static Task<JsonHttpResult<ApiResponse>> GetMostRecent(
		IPacketService packetService,
		ILogService logService,
		HttpContext httpContext)
	{
		return GenericEndpoint(packetService.GetMostRecentShooting, logService, httpContext);
	}

    private static Task<JsonHttpResult<ApiResponse>> GetOldest(
        IPacketService packetService,
        ILogService logService,
        HttpContext httpContext)
    {
        return GenericEndpoint(packetService.GetOldestNotSentTimestamp, logService, httpContext);
    }

    private static async Task<Results<FileContentHttpResult, JsonHttpResult<ApiResponse>>> GetImageFromIDAndCamera(
        int shootingID,
        int cameraID,
        IPacketService packetService,
        ILogService logService,
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
			return await new ApiResponse().ErrorResult(logService, httpContext, e);
		}

		httpContext.Response.Headers.Append("Access-Control-Expose-Headers", "Content-Disposition");
		return TypedResults.File(image, "image/jpeg", ts.ToUnixTimeMilliseconds().ToString());
	}
}