using Core.Entities.IOT.IOTTags.Models.DB;
using Core.Entities.IOT.IOTTags.Models.DTO;
using Core.Entities.IOT.IOTTags.Models.Structs;
using Core.Entities.IOT.IOTTags.Repositories;
using Core.Shared.Services.Kernel;
using Core.Shared.UnitOfWork.Interfaces;

namespace Core.Entities.IOT.IOTTags.Services;

public class IOTTagService : BaseEntityService<IIOTTagRepository, IOTTag, DTOIOTTag>, IIOTTagService
{
	public IOTTagService(IAnodeUOW anodeUOW) : base(anodeUOW)
	{
	}

	public async Task<List<DTOIOTTag>> GetByArrayRID(IEnumerable<string> rids)
	{
		return (await _anodeUOW.IOTTag.GetAll([tag => rids.Contains(tag.RID)], withTracking: false)).ConvertAll(
			tag => tag.ToDTO());
	}

	public async Task<DTOIOTTag> UpdateTagByRID(string rid, string value)
	{
		IOTTag tag = await _anodeUOW.IOTTag.GetByWithThrow([tag => tag.RID == rid], withTracking: false);
		if (tag.IsReadOnly)
		{
			throw new InvalidOperationException(
				"Trying to write a ReadOnly tag. Other changes have been discarded.");
		}

		tag.HasNewValue = true;
		tag.NewValue = value;
		await _anodeUOW.StartTransaction();
		_anodeUOW.IOTTag.Update(tag);
		_anodeUOW.Commit();
		await _anodeUOW.CommitTransaction();
		return tag.ToDTO();
	}

	public async Task<List<DTOIOTTag>> UpdateTags(IEnumerable<PatchIOTTag> updateList)
	{
		List<IOTTag> updatedTags = [];
		await _anodeUOW.StartTransaction();
		foreach (PatchIOTTag patchTag in updateList)
		{
			// await in loop bc we cannot use context concurrently.
			IOTTag tag = await _anodeUOW.IOTTag.GetById(
				patchTag.ID,
				withTracking: false);
			if (tag.IsReadOnly)
			{
				throw new InvalidOperationException(
					"Trying to write a ReadOnly tag. Other changes have been discarded.");
			}

			tag.NewValue = patchTag.NewValue;
			tag.HasNewValue = tag.NewValue != tag.CurrentValue;
			updatedTags.Add(tag);
			_anodeUOW.IOTTag.Update(tag);
			//AnodeUOW.IOTDevice.StopTracking(tag.IOTDevice);
		}

		if (updatedTags.Count == 0)
			return [];

		_anodeUOW.Commit(true);
		await _anodeUOW.CommitTransaction();
		return updatedTags.ToList().ConvertAll(tag => tag.ToDTO());
	}
}