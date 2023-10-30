using Core.Entities.BI.BITemperatures.Models.DB;
using Core.Entities.BI.BITemperatures.Models.DTO;
using Core.Shared.Services.Kernel.Interfaces;

namespace Core.Entities.BI.BITemperatures.Services;

public interface IBITemperatureService : IServiceBaseEntity<BITemperature, DTOBITemperature>
{
	public Task<List<DTOBITemperature>> GetAll();
	public Task LogNewValues();
	public Task PurgeByTimestamp(TimeSpan lifespan);
}