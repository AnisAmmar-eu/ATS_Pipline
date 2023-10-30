using Core.Entities.BI.BITemperatures.Models.DB;
using Core.Entities.BI.BITemperatures.Models.DTO;
using Core.Shared.Data;
using Core.Shared.Repositories.Kernel;

namespace Core.Entities.BI.BITemperatures.Repositories;

public class BITemperatureRepository : RepositoryBaseEntity<AnodeCTX, BITemperature, DTOBITemperature>,
	IBITemperatureRepository
{
	public BITemperatureRepository(AnodeCTX context) : base(context)
	{
	}
}