using Core.Entities.Packets.Models.DTO;
using Core.Entities.StationCycles.Dictionaries;
using Core.Entities.StationCycles.Models.DB.S5Cycles;
using Core.Migrations;
using Core.Shared.Exceptions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;

namespace Core.Entities.StationCycles.Models.DTO.Binders;

public class DTOStationCycleBinder : IModelBinder
{
	
	public async Task BindModelAsync(ModelBindingContext bindingContext)
	{
		try
		{
			Stream stream = bindingContext.HttpContext.Request.Body;

			string json = await new StreamReader(stream).ReadToEndAsync();

			DTOStationCycle stationCycle = JsonConvert.DeserializeObject<DTOStationCycle>(json)!;
			Type dtoType = GetDTOType(stationCycle.CycleType);
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
			throw new EntityNotFoundException("StationCycle type is null");

		return type switch
		{
			CycleTypes.S1S2 => typeof(S1S2Cycle),
			CycleTypes.S3S4 => typeof(S3S4Cycle),
			CycleTypes.S5 => typeof(S5Cycle),
			_ => typeof(DTOPacket)
		};
	}
}