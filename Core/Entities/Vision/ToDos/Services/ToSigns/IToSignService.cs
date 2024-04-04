using Core.Entities.StationCycles.Models.DB;
using Core.Entities.StationCycles.Models.DB.LoadableCycles.S1S2Cycles;
using Core.Entities.Vision.Dictionaries;
using Core.Entities.Vision.ToDos.Models.DB.ToSigns;
using Core.Entities.Vision.ToDos.Models.DTO.ToSigns;
using Core.Shared.Services.Kernel.Interfaces;

namespace Core.Entities.Vision.ToDos.Services.ToSigns;

public interface IToSignService : IBaseEntityService<ToSign, DTOToSign>
{
	void AddAnode(S1S2Cycle cycle);
	Task<StationCycle> UpdateCycle(ToSign toSign, int retSign);
}