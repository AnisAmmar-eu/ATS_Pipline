using Core.Entities.BI.BITemperatures.Models.DB;
using Core.Entities.BI.BITemperatures.Models.DTO;
using Core.Shared.Repositories.Kernel.Interfaces;

namespace Core.Entities.BI.BITemperatures.Repositories;

public interface IBITemperatureRepository : IRepositoryBaseEntity<BITemperature, DTOBITemperature>;