using Core.Entities.IOT.IOTTags.Models.DB;
using Core.Entities.IOT.IOTTags.Models.DTO;
using Core.Shared.Data;
using Core.Shared.Exceptions;
using Core.Shared.Repositories.Kernel;
using Microsoft.EntityFrameworkCore;

namespace Core.Entities.IOT.IOTTags.Repositories;

public class IOTTagRepository : BaseEntityRepository<AnodeCTX, IOTTag, DTOIOTTag>, IIOTTagRepository
{
	public IOTTagRepository(AnodeCTX context) : base(context)
	{
	}

	public IOTTag GetByRIDSync(string rid, bool withTracking = true)
	{
		IQueryable<IOTTag> query = Context.Set<IOTTag>().Where(tag => tag.RID == rid);
		if (!withTracking)
			query.AsNoTracking();

		return query.FirstOrDefault() ?? throw new EntityNotFoundException(nameof(IOTTag));
	}

	public IOTTag GetByIdSync(int id, bool withTracking = true)
	{
		IQueryable<IOTTag> query = Context.Set<IOTTag>().Where(tag => tag.ID == id);
		if (!withTracking)
			query.AsNoTracking();

		return query.FirstOrDefault() ?? throw new EntityNotFoundException(nameof(IOTTag));
	}
}