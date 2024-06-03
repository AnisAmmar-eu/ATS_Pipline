using System.Reflection;
using System.Text.Json;
using Core.Entities.Anodes.Dictionaries;
using Core.Entities.Anodes.Models.DB;
using Core.Entities.Anodes.Models.DTO.AnodesD20;
using Core.Entities.Anodes.Models.DTO.AnodesDX;
using Core.Shared.Exceptions;
using Microsoft.AspNetCore.Http;

namespace Core.Entities.Anodes.Models.DTO;

public partial class DTOAnode
{
	public DTOAnode()
	{
	}

	public override Anode ToModel()
	{
		return this switch {
			DTOAnodeD20 anodeD20 => anodeD20.ToModel(),
			DTOAnodeDX anodeDX => anodeDX.ToModel(),
			_ => throw new InvalidCastException("Trying to convert an abstract class to model"),
		};
	}

	public static async ValueTask<DTOAnode?> BindAsync(HttpContext context, ParameterInfo parameter)
	{
		try
		{
			Stream stream = context.Request.Body;

			string json = await new StreamReader(stream).ReadToEndAsync();

			DTOAnode anode = JsonSerializer.Deserialize<DTOAnode>(json)!;
			Type dtoType = GetDTOType(anode.AnodeType);
			object? formattedModel = JsonSerializer.Deserialize(json, dtoType);
			return formattedModel as DTOAnode;
		}
		catch (Exception e)
		{
			throw new InvalidCastException($"DTO Binding of {nameof(DTOAnode)} has failed: {e}");
		}
	}

	private static Type GetDTOType(string? type)
	{
		return (type is null)
			? throw new EntityNotFoundException("Packet type is null")
			: type switch {
				AnodeTypes.D20 => typeof(DTOAnodeD20),
				AnodeTypes.DX => typeof(DTOAnodeDX),
				AnodeTypes.UNDEFINED => typeof(DTOAnode),
				_ => typeof(DTOAnode),
			};
	}
}