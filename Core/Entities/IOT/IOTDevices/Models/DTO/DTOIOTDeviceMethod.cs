using Core.Entities.Anodes.Dictionaries;
using Core.Entities.Anodes.Models.DTO.AnodesD20;
using Core.Entities.Anodes.Models.DTO.AnodesDX;
using Core.Entities.Anodes.Models.DTO;
using System.Reflection;
using System.Text.Json;
using Core.Entities.IOT.IOTDevices.Models.DB;
using Core.Shared.Exceptions;
using Microsoft.AspNetCore.Http;
using Core.Entities.IOTDevices.Dictionaries;
using Core.Entities.IOT.IOTDevices.Models.DB.ServerRules;

namespace Core.Entities.IOT.IOTDevices.Models.DTO;

public partial class DTOIOTDevice
{
	public DTOIOTDevice()
	{
	}

	public DTOIOTDevice(IOTDevice iotDevice)
	{
		ID = iotDevice.ID;
		TS = iotDevice.TS;
		RID = iotDevice.RID;
		Name = iotDevice.Name;
		Description = iotDevice.Description;
		Address = iotDevice.Address;
		IsConnected = iotDevice.IsConnected;
		IOTTags = iotDevice.IOTTags.ConvertAll(tag => tag.ToDTO());
	}

	public override IOTDevice ToModel() => new(this);

	public static async ValueTask<DTOIOTDevice?> BindAsync(HttpContext context, ParameterInfo parameter)
	{
		try
		{
			Stream stream = context.Request.Body;

			string json = await new StreamReader(stream).ReadToEndAsync();

			DTOIOTDevice dto = JsonSerializer.Deserialize<DTOIOTDevice>(json)!;
			Type dtoType = GetDTOType(dto.Type);
			object? formattedModel = JsonSerializer.Deserialize(json, dtoType);
			return formattedModel as DTOIOTDevice;
		}
		catch (Exception e)
		{
			throw new InvalidCastException($"DTO Binding of {nameof(DTOIOTDevice)} has failed: {e}");
		}
	}

	private static Type GetDTOType(string? type)
	{
		return (type is null)
			? throw new EntityNotFoundException("Packet type is null")
			: type switch {
				IOTDevicesTypes.ServerRule => typeof(ServerRule),
				_ => typeof(DTOIOTDevice),
			};
	}
}