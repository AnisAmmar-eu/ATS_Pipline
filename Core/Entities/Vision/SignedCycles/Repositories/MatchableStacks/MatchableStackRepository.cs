using Core.Entities.Vision.SignedCycles.Dictionaries;
using Core.Entities.Vision.SignedCycles.Models.DB.MatchableStacks;
using Core.Entities.Vision.SignedCycles.Models.DTO.MatchableStacks;
using Core.Shared.Data;
using Core.Shared.Exceptions;
using Core.Shared.Repositories.Kernel;

namespace Core.Entities.Vision.SignedCycles.Repositories.MatchableStacks;

public class MatchableStackRepository : BaseEntityRepository<AnodeCTX, MatchableStack, DTOMatchableStack>,
	IMatchableStackRepository
{
	public MatchableStackRepository(AnodeCTX context) : base(context)
	{
	}

	public async Task<MatchableStack?> Peek(DataSetID dataSetID)
	{
		try
		{
			return await GetBy(
				filters: [matchable => matchable.DataSetID == dataSetID],
				orderBy: query => query.OrderBy(matchable => matchable.CycleTS),
				withTracking: false,
				includes: nameof(MatchableStack.MatchableCycle));
		}
		catch (EntityNotFoundException)
		{
			return null;
		}
	}
}