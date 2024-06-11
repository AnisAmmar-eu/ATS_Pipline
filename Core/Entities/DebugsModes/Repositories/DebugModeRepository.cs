using Core.Entities.DebugsModes.Models.DB;
using Core.Entities.DebugsModes.Models.DTO;
using Core.Shared.Data;
using Core.Shared.Repositories.Kernel;

namespace Core.Entities.DebugsModes.Repositories;

public class DebugModeRepository : BaseEntityRepository<AnodeCTX, DebugMode, DTODebugMode>, IDebugModeRepository
{
	public DebugModeRepository(AnodeCTX context) : base(context, [], [])
	{
	}
}