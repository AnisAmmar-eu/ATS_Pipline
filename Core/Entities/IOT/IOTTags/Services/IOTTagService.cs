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
		return (await AnodeUOW.IOTTag.GetBy(filters: new Expression<Func<IOTTag, bool>>[]
		{
			tag => tag.RID == rid
		}, withTracking: false)).ToDTO();
	}

	public async Task<List<DTOIOTTag>> GetByArrayRID(IEnumerable<string> rids)
	{
		return (await AnodeUOW.IOTTag.GetAll(filters: new Expression<Func<IOTTag, bool>>[]
		{
			tag => rids.Contains(tag.RID)
		}, withTracking: false)).ConvertAll(tag => tag.ToDTO());
	}

	public async Task<bool> IsTestModeOn()
	{
		IOTTag testModeTag;
		if (_testModeID == null)
		{
			testModeTag = await AnodeUOW.IOTTag.GetBy(filters: new Expression<Func<IOTTag, bool>>[]
			{
				tag => tag.RID == IOTTagNames.TestModeName
			}, withTracking: false);
			_testModeID = testModeTag.ID;
		}
		else testModeTag = await AnodeUOW.IOTTag.GetById(_testModeID.Value, withTracking: false);

		return bool.Parse(testModeTag.CurrentValue);
	}

	public async Task<List<DTOIOTTag>> UpdateTags(IEnumerable<PatchIOTTag> updateList)
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