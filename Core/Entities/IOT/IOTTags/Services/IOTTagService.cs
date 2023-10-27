using System.Linq.Expressions;
using Core.Entities.IOT.Dictionaries;
using Core.Entities.IOT.IOTTags.Models.DB;
using Core.Entities.IOT.IOTTags.Models.DTO;
using Core.Entities.IOT.IOTTags.Models.Structs;
using Core.Entities.IOT.IOTTags.Repositories;
using Core.Shared.Services.Kernel;
using Core.Shared.UnitOfWork.Interfaces;

namespace Core.Entities.IOT.IOTTags.Services;

public class IOTTagService : ServiceBaseEntity<IIOTTagRepository, IOTTag, DTOIOTTag>, IIOTTagService
{
	private static int? _testModeID;

	public IOTTagService(IAnodeUOW anodeUOW) : base(anodeUOW)
	{
	}

	public async Task<DTOIOTTag> GetByRID(string rid)
	{
		return (await AnodeUOW.IOTTag.GetBy(new Expression<Func<IOTTag, bool>>[]
		{
			tag => tag.RID == rid
		}, withTracking: false)).ToDTO();
	}

	public async Task<List<DTOIOTTag>> GetByArrayRID(IEnumerable<string> rids)
	{
		return (await AnodeUOW.IOTTag.GetAll(new Expression<Func<IOTTag, bool>>[]
		{
			tag => rids.Contains(tag.RID)
		}, withTracking: false)).ConvertAll(tag => tag.ToDTO());
	}

	public async Task<bool> IsTestModeOn()
	{
		IOTTag testModeTag;
		if (_testModeID == null)
		{
			testModeTag = await AnodeUOW.IOTTag.GetBy(new Expression<Func<IOTTag, bool>>[]
			{
				tag => tag.RID == IOTTagRID.TestMode
			}, withTracking: false);
			_testModeID = testModeTag.ID;
		}
		else
		{
			testModeTag = await AnodeUOW.IOTTag.GetById(_testModeID.Value, withTracking: false);
		}

		return bool.Parse(testModeTag.CurrentValue);
	}

	public async Task<List<DTOIOTTag>> UpdateTags(IEnumerable<PatchIOTTag> updateList)
	{
		List<IOTTag> updatedTags = new();
		await AnodeUOW.StartTransaction();
		foreach (PatchIOTTag patchTag in updateList)
		{
			// await in loop bc we cannot use context concurrently.
			IOTTag tag = await AnodeUOW.IOTTag.GetById(patchTag.ID, withTracking: false);
			if (tag.IsReadOnly)
				throw new InvalidOperationException(
					"Trying to write a ReadOnly tag. Other changes have been discarded.");
			tag.NewValue = patchTag.NewValue;
			tag.HasNewValue = tag.NewValue != tag.CurrentValue;
			updatedTags.Add(tag);
			AnodeUOW.IOTTag.Update(tag);
			//AnodeUOW.IOTDevice.StopTracking(tag.IOTDevice);
		}

		if (!updatedTags.Any())
			return new List<DTOIOTTag>();
		AnodeUOW.Commit();
		await AnodeUOW.CommitTransaction();
		return updatedTags.ToList().ConvertAll(tag => tag.ToDTO());
	}
}