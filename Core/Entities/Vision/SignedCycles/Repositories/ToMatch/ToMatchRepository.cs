using Core.Entities.Vision.SignedCycles.Dictionaries;
using Core.Entities.Vision.SignedCycles.Models.DB.ToMatchs;
using Core.Entities.Vision.SignedCycles.Models.DTO.ToMatchs;
using Core.Shared.Data;
using Core.Shared.Exceptions;
using Core.Shared.Repositories.Kernel;

namespace Core.Entities.Vision.SignedCycles.Repositories.ToMatchs;

public class ToMatchRepository : BaseEntityRepository<AnodeCTX, ToMatch, DTOToMatch>,
	IToMatchRepository
{
	public ToMatchRepository(AnodeCTX context) : base(context)
	{
	}

	public async Task<ToMatch?> Peek(DataSetID dataSetID)
	{
		try
		{
			return await GetBy(
				filters: [matchable => matchable.DataSetID == dataSetID],
				orderBy: query => query.OrderBy(matchable => matchable.CycleTS),
				withTracking: false,
				includes: nameof(ToMatch.MatchableCycle));
		}
		catch (EntityNotFoundException)
		{
			return null;
		}
	}
}