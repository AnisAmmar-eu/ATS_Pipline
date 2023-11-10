using System.Linq.Expressions;
using Core.Entities.BI.BITemperatures.Models.DB;
using Core.Entities.BI.BITemperatures.Models.DTO;
using Core.Entities.BI.BITemperatures.Repositories;
using Core.Entities.IOT.Dictionaries;
using Core.Entities.IOT.IOTTags.Models.DB;
using Core.Shared.Services.Kernel;
using Core.Shared.UnitOfWork.Interfaces;

namespace Core.Entities.BI.BITemperatures.Services;

public class BITemperatureService : ServiceBaseEntity<IBITemperatureRepository, BITemperature, DTOBITemperature>,
	IBITemperatureService
{
	private readonly string[] _temperatureTagsRIDs = { $"{IOTTagRID.Temperature}{1}", $"{IOTTagRID.Temperature}{2}" };

	public BITemperatureService(IAnodeUOW anodeUOW) : base(anodeUOW)
	{
	}

	public async Task<List<DTOBITemperature>> GetAll()
	{
		return (await AnodeUOW.BITemperature.GetAll(withTracking: false)).ConvertAll(
			temperature => temperature.ToDTO());
	}

	public async Task LogNewValues()
	{
		List<IOTTag> temperatureTags = await AnodeUOW.IOTTag.GetAll(new Expression<Func<IOTTag, bool>>[]
		{
			tag => _temperatureTagsRIDs.Contains(tag.RID)
		}, withTracking: false);
		if (!temperatureTags.Any())
			return;
		await AnodeUOW.StartTransaction();
		foreach (IOTTag tag in temperatureTags)
			await AnodeUOW.BITemperature.Add(new BITemperature(tag));

		AnodeUOW.Commit();
		await AnodeUOW.CommitTransaction();
	}

	public async Task PurgeByTimestamp(TimeSpan lifespan)
	{
		DateTimeOffset threshold = DateTimeOffset.Now.Subtract(lifespan);
		List<BITemperature> toPurge = await AnodeUOW.BITemperature.GetAll(
			new Expression<Func<BITemperature, bool>>[]
			{
				kpiTemperature => kpiTemperature.TS < threshold
			}, withTracking: false);
		await AnodeUOW.StartTransaction();
		foreach (BITemperature kpiTemperature in toPurge)
			AnodeUOW.BITemperature.Remove(kpiTemperature);
		AnodeUOW.Commit();
		await AnodeUOW.CommitTransaction();
	}
}