using Core.Entities.IOT.IOTTags.Models.DB;
using Core.Entities.IOT.IOTTags.Models.DTO;
using Core.Entities.IOT.IOTTags.Models.Structs;
using Core.Shared.Services.Kernel.Interfaces;

namespace Core.Entities.IOT.IOTTags.Services;

public interface IIOTTagService : IServiceBaseEntity<IOTTag, DTOIOTTag>
{
	public Task<DTOIOTTag> GetByRID(string rid);
	public Task<bool> IsTestModeOn();
	public Task<List<DTOIOTTag>> UpdateTags(IEnumerable<PatchIOTTag> updateList);
}