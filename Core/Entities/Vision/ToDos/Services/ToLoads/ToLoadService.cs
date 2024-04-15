using Core.Entities.IOT.IOTDevices.Models.DB;
using Core.Entities.IOT.IOTDevices.Models.DB.BackgroundServices.Match;
using Core.Entities.Vision.ToDos.Models.DB.ToLoads;
using Core.Entities.Vision.ToDos.Models.DTO.ToLoads;
using Core.Entities.Vision.ToDos.Repositories.ToLoads;
using Core.Shared.Services.Kernel;
using Core.Shared.UnitOfWork.Interfaces;

namespace Core.Entities.Vision.ToDos.Services.ToLoads;

public class ToLoadService :
	BaseEntityService<IToLoadRepository, ToLoad, DTOToLoad>,
	IToLoadService
{
	public ToLoadService(IAnodeUOW anodeUOW) : base(anodeUOW)
	{
	}

	public static async Task<List<int>> GetInstances(string family, IAnodeUOW anodeUOW)
	{
		try
		{
			return (await anodeUOW.IOTDevice
				.GetAll([device => device is Match], withTracking: false))
				.Cast<Match>()
				.Where(match => match.Family == family)
				.Select(match => match.InstanceMatchID)
				.ToList();
		}
		catch (Exception)
		{
			throw;
		}
	}
}