using Core.Entities.IOT.IOTTags.Models.DB;
using Core.Entities.IOT.IOTTags.Models.DTO;
using Core.Entities.IOT.IOTTags.Models.Structs;
using Core.Shared.Services.Kernel.Interfaces;

namespace Core.Entities.IOT.IOTTags.Services;

public interface IIOTTagService : IBaseEntityService<IOTTag, DTOIOTTag>
{
	public Task<List<DTOIOTTag>> GetByArrayRID(IEnumerable<string> rids);
	public bool IsTestModeOnSync();
	public Task<DTOIOTTag> UpdateTagByRID(string rid, string value);
	public Task<List<DTOIOTTag>> UpdateTags(IEnumerable<PatchIOTTag> updateList);
}