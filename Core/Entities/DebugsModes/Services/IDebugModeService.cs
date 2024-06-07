using Core.Entities.DebugsModes.Models.DB;
using Core.Entities.DebugsModes.Models.DTO;
using Core.Shared.Services.Kernel.Interfaces;

namespace Core.Entities.DebugsModes.Services;

public interface IDebugModeService : IBaseEntityService<DebugMode, DTODebugMode>
{
	Task<bool> ApplyDebugMode(bool enabled);
	Task<bool> ApplyLog(bool enabled, string severity);
	Task<bool> ApplyCsvExport(bool enabled);
}