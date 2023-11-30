using System.Text.Json;
using Core.Entities.Packets.Dictionaries;
using Core.Entities.Packets.Models.DB;
using Core.Entities.Packets.Models.DB.Announcements.S1S2Announcement;
using Core.Entities.Packets.Models.DTO.AlarmLists;
using Core.Entities.Packets.Models.DTO.Announcements;
using Core.Entities.Packets.Models.DTO.Detections;
using Core.Entities.Packets.Models.DTO.Furnaces.InFurnaces;
using Core.Entities.Packets.Models.DTO.Furnaces.OutFurnaces;
using Core.Entities.Packets.Models.DTO.Shootings;
using Core.Shared.Exceptions;
using Microsoft.AspNetCore.Http;

namespace Core.Entities.Packets.Models.DTO;

public partial class DTOPacket
{
	public DTOPacket()
	{
		StationCycleRID = string.Empty;
		Type = string.Empty;
		Status = string.Empty;
	}

	public DTOPacket(Packet detection)
	{
		Type = string.Empty;
		ID = detection.ID;
		TS = detection.TS;
		StationCycleRID = detection.StationCycleRID;
		Status = detection.Status;
		HasError = detection.HasError;
	}

	public override Packet ToModel()
	{
		return new(this);
	}

	public static async ValueTask<DTOPacket?> BindAsync(HttpContext httpContext)
	{
		try
		{
			Stream stream = httpContext.Request.Body;

			string json = await new StreamReader(stream).ReadToEndAsync();

			DTOPacket packet = JsonSerializer.Deserialize<DTOPacket>(json)!;
			Type dtoType = GetDTOType(packet.Type);
			object? formattedModel = JsonSerializer.Deserialize(json, dtoType);
			return formattedModel as DTOPacket;
		}
		catch
		{
			throw new InvalidCastException($"DTO Binding of {nameof(DTOPacket)} has failed.");
		}
	}

	private static Type GetDTOType(string? type)
	{
		if (type is null)
			throw new EntityNotFoundException("Packet type is null");

		return type switch {
			PacketType.Alarm => typeof(DTOAlarmList),
			PacketType.Announcement => typeof(DTOAnnouncement),
			PacketType.S1S2Announcement => typeof(S1S2Announcement),
			PacketType.Detection => typeof(DTODetection),
			PacketType.Shooting => typeof(DTOShooting),
			PacketType.InFurnace => typeof(DTOInFurnace),
			PacketType.OutFurnace => typeof(DTOOutFurnace),
			_ => typeof(DTOPacket),
		};
	}
}