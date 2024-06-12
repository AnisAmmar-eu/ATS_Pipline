using Core.Entities.StationCycles.Models.DB.MatchableCycles;
using Core.Entities.Vision.ToDos.Models.DB.ToMatchs;
using Core.Entities.Vision.ToDos.Models.DTO.ToMatchs;
using Core.Shared.Services.Kernel.Interfaces;

namespace Core.Entities.Vision.ToDos.Services.ToMatchs;

public interface IToMatchService : IBaseEntityService<ToMatch, DTOToMatch>
{
	Task<bool> GoMatch(List<string> origins, int instanceMatchID, double delay);
	Task UpdateAnode(MatchableCycle cycle, string? cycleRID);
	Task<MatchableCycle> UpdateCycle(MatchableCycle cycle, nint retMatch, int cameraID, bool isChained);
	Task UpdateChainedCycle(MatchableCycle cycle, string? cycleRID);
}