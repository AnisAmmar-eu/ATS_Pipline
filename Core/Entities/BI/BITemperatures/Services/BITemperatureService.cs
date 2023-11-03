using System.Linq.Expressions;
using Core.Entities.BI.BITemperatures.Models.DB;
using Core.Entities.BI.BITemperatures.Models.DTO;
using Core.Entities.BI.BITemperatures.Repositories;
using Core.Entities.IOT.IOTDevices.Models.DB;
using Core.Entities.IOT.IOTDevices.Models.DB.OTCameras;
using Core.Shared.Services.Kernel;
using Core.Shared.UnitOfWork.Interfaces;

namespace Core.Entities.BI.BITemperatures.Services;

public class BITemperatureService : ServiceBaseEntity<IBITemperatureRepository, BITemperature, DTOBITemperature>,
	IBITemperatureService
{
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
		List<OTCamera> cameras = (await AnodeUOW.IOTDevice.GetAll(new Expression<Func<IOTDevice, bool>>[]
		{
			device => device is OTCamera
		}, withTracking: false)).ConvertAll(device => device as OTCamera)!;
		if (!cameras.Any())
			return;
		await AnodeUOW.StartTransaction();
		foreach (OTCamera camera in cameras)
			await AnodeUOW.BITemperature.Add(new BITemperature(camera));

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