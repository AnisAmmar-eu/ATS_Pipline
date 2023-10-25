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
		IEnumerable<Task<IOTTag>> tasks = updateList.Select(async tuple =>
		{
			IOTTag tag = await AnodeUOW.IOTTag.GetById(tuple.ID, withTracking: false, includes: "IOTDevice");
			if (tag.IsReadOnly)
				throw new InvalidOperationException(
					"Trying to write a ReadOnly tag. Other changes have been discarded.");
			tag.NewValue = tuple.NewValue;
			tag.HasNewValue = tag.NewValue != tag.CurrentValue;
			return tag;
		});
		IOTTag[] updatedTags = await Task.WhenAll(tasks);
		AnodeUOW.IOTTag.UpdateArray(updatedTags);
		AnodeUOW.Commit();
		await AnodeUOW.CommitTransaction();
		return updatedTags.ToList().ConvertAll(tag => tag.ToDTO());
	}
}