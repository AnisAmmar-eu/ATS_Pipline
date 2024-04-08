using Core.Entities.StationCycles.Models.DB;
using Core.Entities.StationCycles.Models.DB.MatchableCycles;
using Core.Entities.Vision.Dictionaries;
using Core.Entities.Vision.ToDos.Models.DB.ToLoads;
using Core.Entities.Vision.ToDos.Models.DB.ToMatchs;
using Core.Entities.Vision.ToDos.Models.DTO.ToMatchs;
using Core.Shared.Services.Kernel.Interfaces;

namespace Core.Entities.Vision.ToDos.Services.ToMatchs;

public interface IToMatchService : IBaseEntityService<ToMatch, DTOToMatch>
{
	void UpdateAnode(MatchableCycle cycle);
	Task<MatchableCycle> UpdateCycle(ToMatch toMatch, IntPtr retMatch, int cameraID);
}