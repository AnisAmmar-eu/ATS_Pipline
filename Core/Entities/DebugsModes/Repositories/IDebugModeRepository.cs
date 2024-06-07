using Core.Entities.DebugsModes.Models.DB;
using Core.Entities.DebugsModes.Models.DTO;
using Core.Shared.Repositories.Kernel.Interfaces;

namespace Core.Entities.DebugsModes.Repositories;

public interface IDebugModeRepository : IBaseEntityRepository<DebugMode, DTODebugMode>;