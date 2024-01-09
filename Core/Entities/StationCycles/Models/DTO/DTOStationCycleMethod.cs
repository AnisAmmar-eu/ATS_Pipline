using System.Text.Json;
using Core.Entities.StationCycles.Dictionaries;
using Core.Entities.StationCycles.Models.DB;
using Core.Entities.StationCycles.Models.DTO.LoadableCycles.S1S2Cycles;
using Core.Entities.StationCycles.Models.DTO.MatchingCycles.S3S4Cycles;
using Core.Entities.StationCycles.Models.DTO.MatchingCycles.S5Cycles;
using Core.Shared.Exceptions;
using Microsoft.AspNetCore.Http;

namespace Core.Entities.StationCycles.Models.DTO;

public partial class DTOStationCycle
{
	public DTOStationCycle()
	{
	}

	public DTOStationCycle(StationCycle stationCycle)
	{
		ID = stationCycle.ID;
		TS = stationCycle.TS;

		StationID = stationCycle.StationID;
		AnodeType = stationCycle.AnodeType;
		RID = stationCycle.RID;
		Status = stationCycle.Status;
		TSClosed = stationCycle.TSClosed;
		SignStatus1 = stationCycle.SignStatus1;
		SignStatus2 = stationCycle.SignStatus2;

		AnnouncementStatus = stationCycle.AnnouncementStatus;
		AnnouncementID = stationCycle.AnnouncementID;
		AnnouncementPacket = stationCycle.AnnouncementPacket?.ToDTO();

		DetectionStatus = stationCycle.DetectionStatus;
		DetectionID = stationCycle.DetectionID;
		DetectionPacket = stationCycle.DetectionPacket?.ToDTO();

		ShootingStatus = stationCycle.ShootingStatus;
		ShootingID = stationCycle.ShootingID;
		ShootingPacket = stationCycle.ShootingPacket?.ToDTO();

		AlarmListStatus = stationCycle.AlarmListStatus;
		AlarmListID = stationCycle.AlarmListID;
		AlarmListPacket = stationCycle.AlarmListPacket?.ToDTO();
	}

	public override StationCycle ToModel()
	{
		return new(this);
	}

	public static async ValueTask<DTOStationCycle?> BindAsync(HttpContext httpContext)
	{
		try
		{
			Stream stream = httpContext.Request.Body;

			string json = await new StreamReader(stream).ReadToEndAsync();

			DTOStationCycle stationCycle = JsonSerializer.Deserialize<DTOStationCycle>(json)!;
			Type dtoType = GetDTOType(stationCycle.CycleType);
			object? formattedModel = JsonSerializer.Deserialize(json, dtoType);
			return formattedModel as DTOStationCycle;
		}
		catch
		{
			throw new InvalidCastException($"DTO Binding of {nameof(DTOStationCycle)} has failed.");
		}
	}

	private static Type GetDTOType(string? type)
	{
		if (type is null)
			throw new EntityNotFoundException("DTOStationCycle type is null");

		return type switch {
			CycleTypes.S1S2 => typeof(DTOS1S2Cycle),
			CycleTypes.S3S4 => typeof(DTOS3S4Cycle),
			CycleTypes.S5 => typeof(DTOS5Cycle),
			_ => typeof(DTOStationCycle),
		};
	}
}