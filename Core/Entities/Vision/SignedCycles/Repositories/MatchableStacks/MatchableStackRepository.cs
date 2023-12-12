using Core.Entities.Vision.SignedCycles.Models.DB.MatchableStacks;
using Core.Entities.Vision.SignedCycles.Models.DTO.MatchableStacks;
using Core.Shared.Data;
using Core.Shared.Repositories.Kernel;

namespace Core.Entities.Vision.SignedCycles.Repositories.MatchableStacks;

public class MatchableStackRepository : BaseEntityRepository<AnodeCTX, MatchableStack, DTOMatchableStack>,
	IMatchableStackRepository
{
	public MatchableStackRepository(AnodeCTX context) : base(context)
	{
	}
}