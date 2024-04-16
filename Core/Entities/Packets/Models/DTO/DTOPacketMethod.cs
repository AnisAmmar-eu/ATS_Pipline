using System.Text.Json;
using Core.Entities.Packets.Dictionaries;
using Core.Entities.Packets.Models.DB;
using Core.Entities.Packets.Models.DTO.AlarmLists;
using Core.Entities.Packets.Models.DTO.Furnaces.InFurnaces;
using Core.Entities.Packets.Models.DTO.Furnaces.OutFurnaces;
using Core.Entities.Packets.Models.DTO.MetaDatas;
using Core.Entities.Packets.Models.DTO.Shootings;
using Core.Shared.Exceptions;
using Microsoft.AspNetCore.Http;

namespace Core.Entities.Packets.Models.DTO;

public partial class DTOPacket
{
	public DTOPacket()
	{
	}

	public DTOPacket(Packet packet)
	{
		ID = packet.ID;
		TS = packet.TS;
		StationCycleRID = packet.StationCycleRID;
		Status = packet.Status;
		TwinCatStatus = packet.TwinCatStatus;
		HasError = packet.HasError;
	}

	public override Packet ToModel() => new(this);

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
		catch (Exception e)
		{
			throw new InvalidCastException($"DTO Binding of {nameof(DTOPacket)} has failed: {e}");
		}
	}

	private static Type GetDTOType(string? type)
	{
		return (type is null)
			? throw new EntityNotFoundException("Packet type is null")
			: type switch {
			PacketTypes.AlarmList => typeof(DTOAlarmList),
			PacketTypes.MetaData => typeof(DTOMetaData),
			PacketTypes.Shooting => typeof(DTOShooting),
			PacketTypes.InFurnace => typeof(DTOInFurnace),
			PacketTypes.OutFurnace => typeof(DTOOutFurnace),
			_ => typeof(DTOPacket),
		};
	}
}