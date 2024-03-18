using Core.Entities.Vision.SignedCycles.Dictionaries;
using Core.Entities.Vision.SignedCycles.Models.DB.ToLoads;
using Core.Entities.Vision.SignedCycles.Models.DTO.ToLoads;
using Core.Shared.Data;
using Core.Shared.Exceptions;
using Core.Shared.Repositories.Kernel;

namespace Core.Entities.Vision.SignedCycles.Repositories.ToLoads;

public class ToLoadRepository : BaseEntityRepository<AnodeCTX, ToLoad, DTOToLoad>,
	IToLoadRepository
{
	public ToLoadRepository(AnodeCTX context) : base(context)
	{
	}

	public async Task<ToLoad?> Peek(DataSetID dataSetID)
	{
		try
		{
			return await GetBy(
				filters: [loadable => loadable.DataSetID == dataSetID],
				orderBy: query => query.OrderBy(loadable => loadable.CycleTS),
				withTracking: false,
				includes: nameof(ToLoad.LoadableCycle));
		}
		catch (EntityNotFoundException)
		{
			return null;
		}
	}
}