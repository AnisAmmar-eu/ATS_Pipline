using Core.Entities.StationCycles.Dictionaries;
using Core.Entities.StationCycles.Models.DB;
using Core.Entities.Vision.Dictionaries;
using Core.Entities.Vision.ToDos.Models.DB.ToLoads;
using Core.Entities.Vision.ToDos.Models.DB.ToSigns;
using Core.Entities.Vision.ToDos.Models.DTO.ToSigns;
using Core.Entities.Vision.ToDos.Repositories.ToSigns;
using Core.Shared.Services.Kernel;
using Core.Shared.UnitOfWork.Interfaces;

namespace Core.Entities.Vision.ToDos.Services.ToSigns;

public class ToSignService : BaseEntityService<IToSignRepository, ToSign, DTOToSign>,
	IToSignService
{
	public ToSignService(IAnodeUOW anodeUOW) : base(anodeUOW)
	{
	}

	public async Task<StationCycle> UpdateCycle(ToSign toSign, int retSign)
	{
		StationCycle cycle = await AnodeUOW.StationCycle.GetById(toSign.CycleID);

		if (retSign == 0)
		{
			if (toSign.CameraID == 1)
				cycle.SignStatus1 = SignMatchStatus.Ok;

			if (toSign.CameraID == 2)
				cycle.SignStatus2 = SignMatchStatus.Ok;
		}
		else
		{
			if (toSign.CameraID == 1)
				cycle.SignStatus1 = SignMatchStatus.NotOk;

			if (toSign.CameraID == 2)
				cycle.SignStatus2 = SignMatchStatus.NotOk;
		}

		AnodeUOW.StationCycle.Update(cycle);
		return cycle;
	}
}