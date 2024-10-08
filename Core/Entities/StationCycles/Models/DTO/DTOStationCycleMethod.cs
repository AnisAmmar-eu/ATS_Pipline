using System.Reflection;
using System.Text.Json;
using Core.Entities.StationCycles.Dictionaries;
using Core.Entities.StationCycles.Models.DB;
using Core.Entities.StationCycles.Models.DTO.LoadableCycles.S1S2Cycles;
using Core.Entities.StationCycles.Models.DTO.MatchingCycles.S3S4Cycles;
using Core.Entities.StationCycles.Models.DTO.MatchingCycles.S5Cycles;
using Core.Shared.Exceptions;
using Mapster;
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
		SerialNumber = stationCycle.SerialNumber;
		Status = stationCycle.Status;
		TSFirstShooting = stationCycle.TSFirstShooting;
		SignStatus1 = stationCycle.SignStatus1;
		SignStatus2 = stationCycle.SignStatus2;

		MetaDataID = stationCycle.MetaDataID;
		MetaDataPacket = stationCycle.MetaDataPacket?.ToDTO();

		Picture1Status = stationCycle.Picture1Status;
		Shooting1ID = stationCycle.Shooting1ID;
		Shooting1Packet = stationCycle.Shooting1Packet?.ToDTO();

		Picture2Status = stationCycle.Picture2Status;
		Shooting2ID = stationCycle.Shooting2ID;
		Shooting2Packet = stationCycle.Shooting2Packet?.ToDTO();

		AlarmListID = stationCycle.AlarmListID;
		AlarmListPacket = stationCycle.AlarmListPacket?.ToDTO();
	}

	public override StationCycle ToModel()
	{
		StationCycle stationCycle = this.Adapt<StationCycle>();
		stationCycle.MetaDataPacket = MetaDataPacket?.ToModel();
		stationCycle.Shooting1Packet = Shooting1Packet?.ToModel();
		stationCycle.Shooting2Packet = Shooting2Packet?.ToModel();
		stationCycle.AlarmListPacket = AlarmListPacket?.ToModel();
		return stationCycle;
	}

	public static async ValueTask<DTOStationCycle?> BindAsync(HttpContext context, ParameterInfo parameter)
	{
		try
		{
			Stream stream = context.Request.Body;

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
		return (type is null)
			? throw new EntityNotFoundException("DTOStationCycle type is null")
			: type switch {
				CycleTypes.S1S2 => typeof(DTOS1S2Cycle),
				CycleTypes.S3S4 => typeof(DTOS3S4Cycle),
				CycleTypes.S5 => typeof(DTOS5Cycle),
				_ => typeof(DTOStationCycle),
			};
	}
}