using Core.Entities.StationCycles.Models.DB;
using Core.Entities.Vision.ToDos.Dictionaries;
using Core.Entities.Vision.ToDos.Models.DB.ToSigns;
using Core.Entities.Vision.ToDos.Models.DTO.ToSigns;
using Core.Shared.Services.Kernel.Interfaces;

namespace Core.Entities.Vision.ToDos.Services.ToSigns;

public interface IToSignService : IBaseEntityService<ToSign, DTOToSign>
{
	void AddAnode(StationCycle cycle);
	Task<StationCycle> UpdateCycle(ToSign toSign, int retSign);
}