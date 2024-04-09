using Core.Entities.Anodes.Models.DB.AnodesDX;
using Core.Entities.Anodes.Models.DB;
using Core.Entities.StationCycles.Dictionaries;
using Core.Entities.Vision.ToDos.Models.DB.ToMatchs;
using Core.Entities.Vision.ToDos.Models.DTO.ToMatchs;
using Core.Entities.Vision.ToDos.Repositories.ToMatchs;
using Core.Shared.Exceptions;
using Core.Shared.Services.Kernel;
using Core.Shared.UnitOfWork.Interfaces;
using Core.Entities.StationCycles.Models.DB.MatchableCycles;
using Core.Entities.StationCycles.Models.DB.MatchableCycles.S3S4Cycles;
using Core.Entities.StationCycles.Models.DB.MatchableCycles.S5Cycles;
using Core.Entities.KPIData.KPIs.Models.DB;
using DLLVision;
using Core.Entities.KPIData.TenBestMatchs.Models.DB;
using Core.Entities.Vision.Dictionaries;
using Core.Entities.IOT.IOTDevices.Models.DB.ITApiStations;
using Core.Entities.IOT.IOTDevices.Models.DB.ServerRules;
using Core.Entities.IOT.IOTDevices.Models.DB;
using System.Reactive.Linq;
using Core.Migrations;
using Core.Shared.Dictionaries;
using System.Data;

namespace Core.Entities.Vision.ToDos.Services.ToMatchs;

public class ToMatchService :
	BaseEntityService<IToMatchRepository, ToMatch, DTOToMatch>,
	IToMatchService
{
	public ToMatchService(IAnodeUOW anodeUOW) : base(anodeUOW)
	{
	}

	public async Task<MatchableCycle> UpdateCycle(
		MatchableCycle cycle,
		IntPtr retMatch,
		int cameraID,
		InstanceMatchID instance)
	{
		await AnodeUOW.StartTransaction();

		if (DLLVisionImport.fcx_matchRet_errorCode(retMatch) == 0)
		{
			if (cycle.MatchingTS is null)
				cycle.MatchingTS = DateTimeOffset.Now;

			if (instance == InstanceMatchID.S5_C)
			{
				if (cameraID == 1)
					((S5Cycle)cycle).ChainMatchingCamera1 = SignMatchStatus.Ok;

				if (cameraID == 2)
					((S5Cycle)cycle).ChainMatchingCamera2 = SignMatchStatus.Ok;
			}
			else
			{
				if (cameraID == 1)
					cycle.MatchingCamera1 = SignMatchStatus.Ok;

				if (cameraID == 2)
					cycle.MatchingCamera2 = SignMatchStatus.Ok;
			}
		}
		else //-106
		{
			if (instance == InstanceMatchID.S5_C)
			{
				if (cameraID == 1)
					((S5Cycle)cycle).ChainMatchingCamera1 = SignMatchStatus.NotOk;

				if (cameraID == 2)
					((S5Cycle)cycle).ChainMatchingCamera2 = SignMatchStatus.NotOk;
			}
			else
			{
				if (cameraID == 1)
					cycle.MatchingCamera1 = SignMatchStatus.NotOk;

				if (cameraID == 2)
					cycle.MatchingCamera1 = SignMatchStatus.NotOk;
			}
		}

		cycle.KPI = CreateKPI(retMatch);

		AnodeUOW.StationCycle.Update(cycle);
		AnodeUOW.Commit();

		await AnodeUOW.CommitTransaction();
		return cycle;
	}

	public async void UpdateAnode(MatchableCycle cycle)
	{
		await AnodeUOW.StartTransaction();

		try
		{
			Anode anode = await AnodeUOW.Anode.GetByWithThrow(
				[anode => anode.CycleRID == cycle.RID]
				);

			if (cycle is S3S4Cycle)
				anode.S3S4Cycle = cycle as S3S4Cycle;
			else
				((AnodeDX)anode).S5Cycle = cycle as S5Cycle;

			AnodeUOW.Commit();
		}
		catch (EntityNotFoundException)
		{
		}
		catch (Exception)
		{
			throw;
		}

		await AnodeUOW.CommitTransaction();
	}

	private KPI CreateKPI(IntPtr retMatch)
	{
		KPI kPI = new();
		kPI.MScore = DLLVisionImport.fcx_matchRet_similarityScore(retMatch);
		kPI.NbCandidats = DLLVisionImport.fcx_matchRet_cardinality_after_brut_force(retMatch); //TODO change when new DLL
		kPI.Threshold = DLLVisionImport.fcx_matchRet_threshold(retMatch);
		kPI.NMminScore = DLLVisionImport.fcx_matchRet_worstScore(retMatch);
		// kPI.NMmaxScore = DLLVisionImport.fcx_matchRet_bestScore(retMatch); TODO allow when DLL works
		//kPI.NMAvg = DLLVisionImport.fcx_matchRet_mean(retMatch);
		//kPI.NMStdev = Math.Sqrt(DLLVisionImport.fcx_matchRet_variance(retMatch));
		kPI.ComputeTime = DLLVisionImport.fcx_matchRet_elapsed(retMatch);

		for (int i = 0; i < 10; i++)
		{
			kPI.TenBestMatches.Add(new TenBestMatch() {
				Rank=i,
				//AnodeID = DLLVisionImport.fcx_matchRet_bestsIdx_anodeId(retMatch, i), TODO allow when DLL works
				//Score = DLLVisionImport.fcx_matchRet_bestsIdx_score(retMatch, i), TODO allow when DLL works
				KPI = kPI, });
		}

		return kPI;
	}

	public async Task<bool> GoMatch(List<string> origins, InstanceMatchID instance, int delay)
	{
		try
		{
			List<IOTDevice> iotDevices = await AnodeUOW.IOTDevice
				.GetAll([device => device is ITApiStation]);
			ServerRule? rule = await AnodeUOW.IOTDevice.GetBy([device => device is ServerRule]) as ServerRule;

			List<int> stationIDs = origins.ConvertAll(Station.StationNameToID);
			DateTimeOffset? oldestToSign = (await AnodeUOW.ToSign
				.GetBy(
					[toSign => stationIDs.Contains(toSign.StationID)],
					orderBy: query => query.OrderByDescending(
						toSign => toSign.ShootingTS)))
				?.ShootingTS
				?? DateTimeOffset.Now;

			DateTimeOffset? oldestToLoad = (await AnodeUOW.ToLoad
				.GetBy(
					[toLoad => toLoad.InstanceMatchID == instance],
					orderBy: query => query.OrderByDescending(
						toLoad => toLoad.ShootingTS)))
				?.ShootingTS
				?? DateTimeOffset.Now;

			DateTimeOffset? oldestStation = DateTimeOffset.Now;
			foreach (string origin in origins)
			{
				DateTimeOffset? newOldestStation
					= (iotDevices.Find(device => device.RID.EndsWith(origin)) as ITApiStation)?.OldestTSShooting;

				if (newOldestStation is not null && newOldestStation > oldestStation)
					oldestStation = newOldestStation;
			}

			return rule is not null
				&& rule!.Reinit
				&& iotDevices.Select(device => device.IsConnected).Contains(false)
				&& ValidDelay(oldestStation, delay)
				&& ValidDelay(oldestToLoad, delay)
				&& ValidDelay(oldestStation, delay);
		}
		catch (Exception)
		{
			throw;
		}
	}

	private bool ValidDelay(DateTimeOffset? date, int delay)
	{
		if (date is null)
			return false;

		return ((DateTimeOffset)date).AddDays(delay) > DateTimeOffset.Now;
	}
}