using Core.Entities.IOT.IOTDevices.Models.DB.BackgroundServices.Matchs;
using Core.Entities.Vision.ToDos.Models.DB.ToUnloads;
using Core.Entities.Vision.ToDos.Models.DTO.ToUnloads;
using Core.Entities.Vision.ToDos.Repositories.ToUnloads;
using Core.Shared.Services.Kernel;
using Core.Shared.UnitOfWork.Interfaces;

namespace Core.Entities.Vision.ToDos.Services.ToUnloads;

public class ToUnloadService :
	BaseEntityService<IToUnloadRepository, ToUnload, DTOToUnload>,
	IToUnloadService
{
	public ToUnloadService(IAnodeUOW anodeUOW) : base(anodeUOW)
	{
	}

	public static async Task<List<int>> GetInstances(int instanceMatchID, IAnodeUOW anodeUOW)
	{
		try
		{
			string? family = ((Match?)await anodeUOW.IOTDevice
				.GetBy(
					[device => device is Match && ((Match)device).InstanceMatchID == instanceMatchID],
		   withTracking: false))
				?.Family;

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