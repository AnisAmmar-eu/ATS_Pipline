using System.Text.Json;
using Core.Entities.StationCycles.Dictionaries;
using Core.Entities.StationCycles.Models.DTO.LoadableCycles.S1S2Cycles;
using Core.Entities.StationCycles.Models.DTO.MatchingCycles.S3S4Cycles;
using Core.Entities.StationCycles.Models.DTO.MatchingCycles.S5Cycles;
using Core.Shared.Exceptions;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Core.Entities.StationCycles.Models.DTO.Binders;

public class DTOStationCycleBinder : IModelBinder
{
	public async Task BindModelAsync(ModelBindingContext bindingContext)
	{
		try
		{
			Stream stream = bindingContext.HttpContext.Request.Body;

			string json = await new StreamReader(stream).ReadToEndAsync();

			DTOStationCycle stationCycle = JsonSerializer.Deserialize<DTOStationCycle>(json)!;
			Type dtoType = GetDTOType(stationCycle.CycleType);
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
			throw new EntityNotFoundException("StationCycle type is null");

		return type switch {
			CycleTypes.S1S2 => typeof(DTOS1S2Cycle),
			CycleTypes.S3S4 => typeof(DTOS3S4Cycle),
			CycleTypes.S5 => typeof(DTOS5Cycle),
			_ => typeof(DTOStationCycle),
		};
	}
}