using Core.Entities.BI.BITemperatures.Models.DB;
using Core.Entities.BI.BITemperatures.Models.DTO;
using Core.Shared.Services.Kernel.Interfaces;

namespace Core.Entities.BI.BITemperatures.Services;

public interface IBITemperatureService : IBaseEntityService<BITemperature, DTOBITemperature>
{
	/// <summary>
	/// Adds a BITemperature row for every camera logging its temperature.
	/// </summary>
	/// <returns></returns>
	public Task LogNewValues();
}