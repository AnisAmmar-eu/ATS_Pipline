using System.ComponentModel.DataAnnotations;
using Carter;
using Core.Entities.Alarms.AlarmsCycle.Models.DTO;
using Core.Entities.Alarms.AlarmsLog.Models.DTO;
using Core.Entities.Alarms.AlarmsLog.Services;
using Core.Entities.Packets.Models.DTO;
using Core.Entities.Packets.Models.DTO.Shootings;
using Core.Entities.Packets.Services;
using Core.Shared.Dictionaries;
using Core.Shared.Endpoints.Kernel;
using Core.Shared.Models.ApiResponses;
using Core.Shared.Models.DTO.System.Logs;
using Core.Shared.Services.System.Logs;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ApiServerReceive.Endpoints;

public class ReceiveEndpoint : BaseEndpoint, ICarterModule
{
	public void AddRoutes(IEndpointRouteBuilder app)
	{
		if (!Station.IsServer)
			return;

		RouteGroupBuilder group = app.MapGroup("apiServerReceive").WithTags(nameof(ReceiveEndpoint));

		group.MapGet("status", () => new ApiResponse().SuccessResult());
		group.MapPost("alarmsLog", ReceiveAlarmLog);
		group.MapPost("{stationName}/packets", ReceivePacket);
		// This one needs more information due to CustomModelBinding requiring the removal of [FromBody]
		group.MapPost("images", ReceiveImage);
		group.MapPost("{stationName}/alarmPacket/{cycleRID}", ReceiveAlarmPacket);
		group.MapPost("logs", ReceiveLog);
	}

	private static Task<JsonHttpResult<ApiResponse>> ReceiveAlarmLog(
		[FromBody] [Required] List<DTOAlarmLog> dtoAlarmLogs,
		IAlarmLogService alarmLogService,
		ILogService logService,
		HttpContext httpContext)
	{
		return GenericEndpointEmptyResponse(
			async () =>
			{
				foreach (DTOAlarmLog alarmLog in dtoAlarmLogs)
					await alarmLogService.ReceiveAlarmLog(alarmLog);
			},
			logService,
			httpContext);
	}

	private static Task<JsonHttpResult<ApiResponse>> ReceivePacket(
		[FromBody] [Required] DTOPacket packet,
		[FromRoute] string stationName,
		IPacketService packetService,
		ILogService logService,
		HttpContext httpContext)
	{
		return GenericEndpointEmptyResponse(
			() => packetService.ReceivePacket(packet, stationName),
			logService,
			httpContext);
	}

	private static Task<JsonHttpResult<ApiResponse>> ReceiveImage(
		IPacketService packetService,
		ILogService logService,
		HttpContext httpContext)
	{
		return GenericEndpointEmptyResponse(
			() =>
			{
				FormFileCollection images = [];
				images.AddRange(
					httpContext.Request.Form.Files.Where(formFile => formFile.ContentType.Contains("image")));
				return packetService.ReceiveStationImage(images);
			},
			logService,
			httpContext);
	}

	private static Task<JsonHttpResult<ApiResponse>> ReceiveAlarmPacket(
		[FromBody] [Required] List<DTOAlarmCycle> dtoAlarmsCycle,
		[FromRoute] string stationName,
		[FromRoute] string cycleRID,
		IPacketService packetService,
		ILogService logService,
		HttpContext httpContext)
	{
		return GenericEndpointEmptyResponse(
			() => packetService.ReceivePacketAlarm(dtoAlarmsCycle, stationName, cycleRID),
			logService,
			httpContext);
	}

	private static Task<JsonHttpResult<ApiResponse>> ReceiveLog(
		[FromBody] [Required] List<DTOLog> logs,
		ILogService logService,
		HttpContext httpContext)
	{
		return GenericEndpointEmptyResponse(
			() => logService.ReceiveLogs(logs),
			logService,
			httpContext);
	}
}