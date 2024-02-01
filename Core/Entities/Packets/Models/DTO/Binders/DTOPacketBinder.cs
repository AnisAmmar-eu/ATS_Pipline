using System.Text.Json;
using Core.Entities.Packets.Dictionaries;
using Core.Entities.Packets.Models.DTO.AlarmLists;
using Core.Entities.Packets.Models.DTO.Furnaces.InFurnaces;
using Core.Entities.Packets.Models.DTO.Furnaces.OutFurnaces;
using Core.Entities.Packets.Models.DTO.Shootings;
using Core.Shared.Exceptions;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Core.Entities.Packets.Models.DTO.Binders;

public class DTOPacketBinder : IModelBinder
{
	public async Task BindModelAsync(ModelBindingContext bindingContext)
	{
		try
		{
			Stream stream = bindingContext.HttpContext.Request.Body;

			string json = await new StreamReader(stream).ReadToEndAsync();

			DTOPacket packet = JsonSerializer.Deserialize<DTOPacket>(json)!;
			Type dtoType = GetDTOType(packet.Type);
			object? formattedModel = JsonSerializer.Deserialize(json, dtoType);

			bindingContext.Result = ModelBindingResult.Success(formattedModel);
		}
		catch
		{
			bindingContext.Result = ModelBindingResult.Failed();
		}
	}

	private static Type GetDTOType(string? type)
	{
		if (type is null)
			throw new EntityNotFoundException("Packet type is null");

		return type switch {
			PacketType.Alarm => typeof(DTOAlarmList),
			PacketType.Shooting => typeof(DTOShooting),
			PacketType.InFurnace => typeof(DTOInFurnace),
			PacketType.OutFurnace => typeof(DTOOutFurnace),
			_ => typeof(DTOPacket),
		};
	}
}