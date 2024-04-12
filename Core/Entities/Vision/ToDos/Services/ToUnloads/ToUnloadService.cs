using Core.Entities.IOT.IOTDevices.Models.DB.BackgroundServices.Match;
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
			return (List<int>)(await anodeUOW.IOTDevice
				.GetAll([device => device is Match], withTracking: false))
				.Cast<Match>()
				.Where(match => match.InstanceMatchID == instanceMatchID)
				.Select(match => match.InstanceMatchID);
		}
		catch (Exception)
		{
			throw;
		}
	}
}