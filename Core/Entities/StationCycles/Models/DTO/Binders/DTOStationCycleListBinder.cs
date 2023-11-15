using Core.Entities.StationCycles.Dictionaries;
using Core.Entities.StationCycles.Models.DTO.MatchingCycles.S3S4Cycles;
using Core.Entities.StationCycles.Models.DTO.MatchingCycles.S5Cycles;
using Core.Entities.StationCycles.Models.DTO.SigningCycles.S1S2Cycles;
using Core.Shared.Exceptions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;

namespace Core.Entities.StationCycles.Models.DTO.Binders;

public class DTOStationCycleListBinder : IModelBinder
{
	public async Task BindModelAsync(ModelBindingContext bindingContext)
	{
		try
		{
			Stream stream = bindingContext.HttpContext.Request.Body;

			string json = await new StreamReader(stream).ReadToEndAsync();

			List<string> stationCycles = JsonConvert.DeserializeObject<List<object>>(json)
				.Select(JsonConvert.SerializeObject).ToList();
			List<DTOStationCycle> result = new();
			foreach (string s in stationCycles)
			{
				DTOStationCycle stationCycle = JsonConvert.DeserializeObject<DTOStationCycle>(s);
				Type dtoType = GetDTOType(stationCycle.CycleType);
				result.Add(JsonConvert.DeserializeObject(s, dtoType) as DTOStationCycle ??
				           throw new InvalidOperationException("Invalid DTOStationCycle"));
			}

			bindingContext.Result = ModelBindingResult.Success(result);
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
			CycleTypes.S1S2 => typeof(DTOS1S2Cycle),
			CycleTypes.S3S4 => typeof(DTOS3S4Cycle),
			CycleTypes.S5 => typeof(DTOS5Cycle),
			_ => typeof(DTOStationCycle)
		};
	}
}