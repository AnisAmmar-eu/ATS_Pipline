using Core.Entities.Vision.SignedCycles.Models.DB.MatchableStacks;
using Core.Entities.Vision.SignedCycles.Models.DTO.MatchableStacks;
using Core.Shared.Repositories.Kernel.Interfaces;

namespace Core.Entities.Vision.SignedCycles.Repositories.MatchableStacks;

public interface IMatchableStackRepository : IBaseEntityRepository<MatchableStack, DTOMatchableStack>;