using Core.Entities.Anodes.Dictionaries;
using Core.Entities.Anodes.Models.DB;
using Core.Entities.Anodes.Models.DB.AnodesD20;
using Core.Entities.Anodes.Models.DB.AnodesDX;
using Core.Entities.StationCycles.Dictionaries;
using Core.Entities.StationCycles.Models.DB;
using Core.Entities.StationCycles.Models.DB.LoadableCycles.S1S2Cycles;
using Core.Entities.Vision.ToDos.Dictionaries;
using Core.Entities.Vision.ToDos.Models.DB.ToLoads;
using Core.Entities.Vision.ToDos.Models.DB.ToSigns;
using Core.Entities.Vision.ToDos.Models.DTO.ToSigns;
using Core.Entities.Vision.ToDos.Repositories.ToSigns;
using Core.Shared.Exceptions;
using Core.Shared.Services.Kernel;
using Core.Shared.UnitOfWork;
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
        await AnodeUOW.StartTransaction();

        StationCycle cycle = await AnodeUOW.StationCycle.GetById(toSign.StationCycleID);

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
        AnodeUOW.Commit();

        await AnodeUOW.CommitTransaction();
        return cycle;
	}

	public async void AddAnode(StationCycle cycle)
	{
		try
		{
			Anode anode =  await AnodeUOW.Anode.GetBy(
				[anode => anode.S1S2CycleID == cycle.ID]
				);
        }
        catch (EntityNotFoundException)
		{
            await AnodeUOW.StartTransaction();

            if (cycle.AnodeType == AnodeTypes.DX)
                AnodeUOW.Anode.Add(new AnodeDX((S1S2Cycle)cycle));
            else if (cycle.AnodeType == AnodeTypes.D20)
                AnodeUOW.Anode.Add(new AnodeD20((S1S2Cycle)cycle));

            AnodeUOW.Commit();
            await AnodeUOW.CommitTransaction();
        }
        catch (Exception)
		{
			throw;
		}
    }
}