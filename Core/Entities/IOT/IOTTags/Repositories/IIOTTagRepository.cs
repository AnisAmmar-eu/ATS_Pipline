using Core.Entities.IOT.IOTTags.Models.DB;
using Core.Entities.IOT.IOTTags.Models.DTO;
using Core.Shared.Repositories.Kernel.Interfaces;

namespace Core.Entities.IOT.IOTTags.Repositories;

public interface IIOTTagRepository : IBaseEntityRepository<IOTTag, DTOIOTTag>
{
	public IOTTag GetByRIDSync(string rid, bool withTracking = true);
	public IOTTag GetByIdSync(int id, bool withTracking = true);
}