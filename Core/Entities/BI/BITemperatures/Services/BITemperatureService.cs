using Core.Entities.BI.BITemperatures.Models.DB;
using Core.Entities.BI.BITemperatures.Models.DTO;
using Core.Entities.BI.BITemperatures.Repositories;
using Core.Entities.IOT.Dictionaries;
using Core.Entities.IOT.IOTTags.Models.DB;
using Core.Shared.Services.Kernel;
using Core.Shared.UnitOfWork.Interfaces;

namespace Core.Entities.BI.BITemperatures.Services;

public class BITemperatureService :
	BaseEntityService<IBITemperatureRepository, BITemperature, DTOBITemperature>,
	IBITemperatureService
{
	private readonly string[] _temperatureTagsRIDs = [IOTTagRID.TemperatureCam1, IOTTagRID.TemperatureCam2];

	public BITemperatureService(IAnodeUOW anodeUOW) : base(anodeUOW)
	{
	}

	public async Task LogNewValues()
	{
		List<IOTTag> temperatureTags = await _anodeUOW.IOTTag
			.GetAll([tag => _temperatureTagsRIDs.Contains(tag.RID)], withTracking: false);
		if (temperatureTags.Count == 0)
			return;

		await _anodeUOW.StartTransaction();
		foreach (IOTTag tag in temperatureTags)
			await _anodeUOW.BITemperature.Add(new BITemperature(tag));

		_anodeUOW.Commit();
		await _anodeUOW.CommitTransaction();
	}
}