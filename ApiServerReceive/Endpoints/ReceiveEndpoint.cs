using System.ComponentModel.DataAnnotations;
using Carter;
using Core.Entities.Alarms.AlarmsC.Models.DTO;
using Core.Entities.Alarms.AlarmsC.Services;
using Core.Entities.Alarms.AlarmsLog.Models.DB;
using Core.Entities.Alarms.AlarmsLog.Models.DTO;
using Core.Entities.Alarms.AlarmsLog.Services;
using Core.Entities.Packets.Models.DTO;
using Core.Entities.Packets.Models.DTO.Furnaces;
using Core.Entities.Packets.Services;
using Core.Entities.StationCycles.Models.DTO;
using Core.Entities.StationCycles.Services;
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

		group.MapGet("status", GetStatus);
		group.MapPost("alarmsLog", ReceiveAlarmLog);
		group.MapPost("packets", ReceivePacket);
		group.MapPost("furnacePackets", ReceiveFurnacePacketForStationCycle);
		// This one needs more information due to CustomModelBinding requiring the removal of [FromBody]
		group.MapPost("stationCycles", ReceiveStationCycle).Accepts<DTOStationCycle>("application/json").WithOpenApi();
		group.MapPost("images", ReceiveImage);
		group.MapPost("logs", ReceiveLog);
	}

	private static JsonHttpResult<ApiResponse> GetStatus()
	{
		return new ApiResponse().SuccessResult();
	}

	private static Task<JsonHttpResult<ApiResponse>> ReceiveAlarmLog(
		[FromBody] [Required] List<DTOAlarmLog> dtoAlarmLogs,
		IAlarmCService alarmCService,
		IAlarmLogService alarmLogService,
		ILogService logService,
		HttpContext httpContext)
	{
		return GenericEndpointEmptyResponse(
			async () =>
			{
				foreach (DTOAlarmLog alarmLog in dtoAlarmLogs)
				{
					DTOAlarmC newAlarmC = await alarmCService.GetByRID(alarmLog.AlarmRID);

					AlarmLog alarmLogToAdd = alarmLog.ToModel();
					alarmLogToAdd.ID = 0;
					alarmLogToAdd.IsAck = false;
					alarmLogToAdd.HasBeenSent = true;
					alarmLogToAdd.AlarmID = newAlarmC.ID;
					await alarmLogService.Add(alarmLogToAdd);
				}
			},
			logService,
			httpContext);
	}

	private static Task<JsonHttpResult<ApiResponse>> ReceivePacket(
		[FromBody] [Required] IEnumerable<DTOPacket> packets,
		IPacketService packetService,
		ILogService logService,
		HttpContext httpContext)
	{
		return GenericEndpointEmptyResponse(
			async () => await packetService.ReceivePackets(packets),
			logService,
			httpContext);
	}

	/// <summary>
	///     This function will take a furnace packet as argument and will build it. Building it will make it associate
	///     to its stationCycle. StationCycle MUST NOT be marked as sent in the server.
	/// </summary>
	/// <param name="dtoPackets">Received packets</param>
	/// <param name="packetService">Packet service</param>
	/// <param name="logService">Log service</param>
	/// <param name="httpContext">Http context</param>
	private static Task<JsonHttpResult<ApiResponse>> ReceiveFurnacePacketForStationCycle(
		[FromBody] [Required] DTOPacket dtoPackets,
		IPacketService packetService,
		ILogService logService,
		HttpContext httpContext)
	{
		return GenericEndpointEmptyResponse(
			async () =>
			{
				if (dtoPackets is not DTOFurnace)
					throw new InvalidOperationException("Given packet MUST be a Furnace packet.");

				dtoPackets.ID = 0;
				await packetService.BuildPacket(dtoPackets.ToModel());
			},
			logService,
			httpContext);
	}

	private static Task<JsonHttpResult<ApiResponse>> ReceiveStationCycle(
		DTOStationCycle dtoStationCycles,
		IStationCycleService stationCycleService,
		ILogService logService,
		HttpContext httpContext)
	{
		return GenericEndpointEmptyResponse(
			async () => await stationCycleService.ReceiveStationCycle(dtoStationCycles), logService, httpContext);
	}

	private static Task<JsonHttpResult<ApiResponse>> ReceiveImage(
		IStationCycleService stationCycleService,
		ILogService logService,
		HttpContext httpContext)
	{
		return GenericEndpointEmptyResponse(
			() =>
			{
				FormFileCollection images = new();
				images.AddRange(
					httpContext.Request.Form.Files.Where(formFile => formFile.ContentType.Contains("image")));
				return stationCycleService.ReceiveStationImage(images);
			},
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