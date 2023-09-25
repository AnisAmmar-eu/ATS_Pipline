using Core.Entities.Packets.Dictionary;
using Core.Entities.Packets.Models.DTO.AlarmListPackets;
using Core.Entities.Packets.Models.DTO.Detection;
using Core.Shared.Exceptions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;

namespace Core.Entities.Packets.Models.DTO.Binders;

public class DTOPacketBinder: IModelBinder
{
	public async Task BindModelAsync(ModelBindingContext bindingContext)
	{
            try
            {
                Stream stream = bindingContext.HttpContext.Request.Body;

                string json = await new StreamReader(stream).ReadToEndAsync();

                DTOPacket packet = JsonConvert.DeserializeObject<DTOPacket>(json)!;
                Type dtoType = GetDTOType(packet.PacketType);
                object? formatedModel = JsonConvert.DeserializeObject(json, dtoType);

                bindingContext.Result = ModelBindingResult.Success(formatedModel);

            }
            catch
            {
                bindingContext.Result = ModelBindingResult.Failed();
            }
	}

	private static Type GetDTOType(string? type)
	{
		if (type == null)
			throw new EntityNotFoundException("Packet type is null");

		return type switch
		{
			PacketType.ALARM => typeof(DTOAlarmListPacket),
			PacketType.DETECTION => typeof(DTODetectionPacket),
			_ => typeof(DTOPacket),
		};
	}
}