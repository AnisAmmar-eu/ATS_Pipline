using Core.Entities.IOT.IOTDevices.Models.DB.BackgroundServices.Matchs;
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
			List<int> titi = (await anodeUOW.IOTDevice
				.GetAll([device => device is Match], withTracking: false))
				.Cast<Match>()
				.Where(match => match.Family == family)
				.Select(match => match.InstanceMatchID)
				.Distinct()
				.ToList();

			List<int> toto = (await anodeUOW.IOTDevice
				.GetAll([device => device is Match], withTracking: false))
				.Cast<Match>()
				.Where(match => match.Family == family)
				.Select(match => match.InstanceMatchID)
				.ToList();
			toto = toto.Distinct().ToList();

			return (await anodeUOW.IOTDevice
				.GetAll([device => device is Match], withTracking: false))
				.Cast<Match>()
				.Where(match => match.Family == family)
				.Select(match => match.InstanceMatchID)
				.Distinct()
				.ToList();
		}
		catch (Exception)
		{
			throw;
		}
	}
}