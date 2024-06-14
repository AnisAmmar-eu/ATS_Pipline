using Core.Entities.Anodes.Models.DB;
using Core.Entities.Anodes.Models.DB.AnodesD20;
using Core.Entities.Anodes.Models.DB.AnodesDX;
using Core.Entities.StationCycles.Dictionaries;
using Core.Entities.StationCycles.Models.DB;
using Core.Entities.StationCycles.Models.DB.LoadableCycles.S1S2Cycles;
using Core.Entities.Vision.ToDos.Models.DB.ToSigns;
using Core.Entities.Vision.ToDos.Models.DTO.ToSigns;
using Core.Entities.Vision.ToDos.Repositories.ToSigns;
using Core.Shared.Dictionaries;
using Core.Shared.Exceptions;
using Core.Shared.Services.Kernel;
using Core.Shared.UnitOfWork.Interfaces;

namespace Core.Entities.Vision.ToDos.Services.ToSigns;

public class ToSignService :
	BaseEntityService<IToSignRepository, ToSign, DTOToSign>,
	IToSignService
{
	public ToSignService(IAnodeUOW anodeUOW) : base(anodeUOW)
	{
	}

	public async Task<StationCycle> UpdateCycle(ToSign toSign, int retSign)
	{
		await _anodeUOW.StartTransaction();

		StationCycle cycle = await _anodeUOW.StationCycle.GetById(toSign.StationCycleID);

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

		_anodeUOW.StationCycle.Update(cycle);
		_anodeUOW.Commit();

		await _anodeUOW.CommitTransaction();
		return cycle;
	}

	public async Task AddAnode(S1S2Cycle cycle)
	{
		try
		{
			Anode anode = await _anodeUOW.Anode.GetByWithThrow(
				[anode => anode.S1S2CycleID == cycle.ID]
				);

			anode.S1S2SignStatus1 = cycle.SignStatus1;
			anode.S1S2SignStatus2 = cycle.SignStatus2;
			anode.S1S2TSFirstShooting = cycle.TSFirstShooting;
		}
		catch (EntityNotFoundException)
		{
			await _anodeUOW.StartTransaction();

			if (cycle.AnodeType == AnodeTypeDict.DX)
				await _anodeUOW.Anode.Add(new AnodeDX(cycle));
			else if (cycle.AnodeType == AnodeTypeDict.D20)
				await _anodeUOW.Anode.Add(new AnodeD20(cycle));

			_anodeUOW.Commit();
			await _anodeUOW.CommitTransaction();
		}
		catch (Exception)
		{
			throw;
		}
	}
}