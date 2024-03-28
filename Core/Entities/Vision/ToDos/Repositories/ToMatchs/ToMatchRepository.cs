using Core.Entities.Vision.ToDos.Dictionaries;
using Core.Entities.Vision.ToDos.Models.DB.ToMatchs;
using Core.Entities.Vision.ToDos.Models.DTO.ToMatchs;
using Core.Shared.Data;
using Core.Shared.Exceptions;
using Core.Shared.Repositories.Kernel;

namespace Core.Entities.Vision.ToDos.Repositories.ToMatchs;

public class ToMatchRepository : BaseEntityRepository<AnodeCTX, ToMatch, DTOToMatch>,
	IToMatchRepository
{
	public ToMatchRepository(AnodeCTX context) : base(context, [], [])
	{
	}

	public async Task<ToMatch?> Peek(int stationID)
	{
		try
		{
			return await GetBy(
				filters: [matchable => matchable.StationID == stationID],
				orderBy: query => query.OrderBy(matchable => matchable.ShootingTS),
				withTracking: false,
				includes: nameof(ToMatch.MatchableCycle));
		}
		catch (EntityNotFoundException)
		{
			return null;
		}
	}
}