using Core.Entities.IOT.IOTTags.Models.DB;
using Core.Entities.IOT.IOTTags.Models.DTO;
using Core.Entities.IOT.IOTTags.Models.Structs;
using Core.Entities.IOT.IOTTags.Repositories;
using Core.Shared.Services.Kernel;
using Core.Shared.UnitOfWork.Interfaces;

namespace Core.Entities.IOT.IOTTags.Services;

public class IOTTagService : ServiceBaseEntity<IIOTTagRepository, IOTTag, DTOIOTTag>, IIOTTagService
{
	public IOTTagService(IAnodeUOW anodeUOW) : base(anodeUOW)
	{
	}

	public async Task<List<DTOIOTTag>> UpdateTags(List<PatchIOTTag> updateList)
	{
		await AnodeUOW.StartTransaction();
		List<DTOIOTTag> dtoTags = new();
		IEnumerable<Task> tasks = updateList.Select(async tuple =>
		{
			IOTTag tag = await AnodeUOW.IOTTag.GetById(tuple.ID, withTracking: false, includes: "IOTDevice");
			tag.NewValue = tuple.NewValue;
			tag.HasNewValue = tag.NewValue != tag.CurrentValue;
			AnodeUOW.IOTTag.Update(tag);
			AnodeUOW.Commit();
			dtoTags.Add(tag.ToDTO());
		});
		await Task.WhenAll(tasks);
		await AnodeUOW.CommitTransaction();
		return dtoTags;
	}
}