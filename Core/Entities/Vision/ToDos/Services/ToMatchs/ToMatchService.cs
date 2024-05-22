using System.Data;
using System.Reactive.Linq;
using Core.Entities.Anodes.Models.DB;
using Core.Entities.Anodes.Models.DB.AnodesDX;
using Core.Entities.IOT.IOTDevices.Models.DB;
using Core.Entities.IOT.IOTDevices.Models.DB.BackgroundServices.Matchs;
using Core.Entities.IOT.IOTDevices.Models.DB.ITApiStations;
using Core.Entities.IOT.IOTDevices.Models.DB.ServerRules;
using Core.Entities.KPIData.KPIs.Models.DB;
using Core.Entities.KPIData.TenBestMatchs.Models.DB;
using Core.Entities.StationCycles.Dictionaries;
using Core.Entities.StationCycles.Models.DB.MatchableCycles;
using Core.Entities.StationCycles.Models.DB.MatchableCycles.S3S4Cycles;
using Core.Entities.StationCycles.Models.DB.MatchableCycles.S5Cycles;
using Core.Entities.Vision.ToDos.Models.DB.ToMatchs;
using Core.Entities.Vision.ToDos.Models.DTO.ToMatchs;
using Core.Entities.Vision.ToDos.Repositories.ToMatchs;
using Core.Shared.Dictionaries;
using Core.Shared.DLLVision;
using Core.Shared.Exceptions;
using Core.Shared.Services.Kernel;
using Core.Shared.UnitOfWork.Interfaces;

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
		bool isChained)
	{
		await _anodeUOW.StartTransaction();

		int retMatchCode = DLLVisionImport.fcx_matchRet_errorCode(retMatch);
		if (retMatchCode == 0)
		{
			cycle.MatchingTS ??= DateTimeOffset.Now;

			if (isChained)
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
			if (isChained)
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
					cycle.MatchingCamera2 = SignMatchStatus.NotOk;
			}
		}

		if (cameraID == 2 || (cameraID == 1 && retMatchCode != -106))
			cycle.KPI = CreateKPI(retMatch);

		_anodeUOW.StationCycle.Update(cycle);
		_anodeUOW.Commit();

		await _anodeUOW.CommitTransaction();
		return cycle;
	}

	public async Task UpdateAnode(MatchableCycle cycle, string? cycleRID, bool isChained)
	{
		if (cycleRID is null)
			return;

		await _anodeUOW.StartTransaction();

		try
		{
			Anode anode = await _anodeUOW.Anode.GetByWithThrow(
				[anode => anode.CycleRID == cycleRID]
				);

			if (cycle is S3S4Cycle)
			{
				anode.S3S4Cycle = cycle as S3S4Cycle;
				anode.S3S4SignStatus1 = cycle.SignStatus1;
				anode.S3S4SignStatus2 = cycle.SignStatus2;
				anode.SS3S4MatchingCamera1 = cycle.MatchingCamera1;
				anode.S3S4MatchingCamera2 = cycle.MatchingCamera2;
				anode.S3S4TSFirstShooting = cycle.TSFirstShooting;
				anode.IsComplete = cycle.AnodeType == AnodeTypeDict.D20;
			}
			else
			{
				((AnodeDX)anode).S5Cycle = cycle as S5Cycle;
				((AnodeDX)anode).SSignStatus1 = cycle.SignStatus1;
				((AnodeDX)anode).S5SignStatus2 = cycle.SignStatus2;
				((AnodeDX)anode).S5MatchingCamera1 = cycle.MatchingCamera1;
				((AnodeDX)anode).S5MatchingCamera2 = cycle.MatchingCamera2;
				((AnodeDX)anode).S5TSFirstShooting = cycle.TSFirstShooting;
				anode.IsComplete = cycle.AnodeType == AnodeTypeDict.DX;

				if (isChained)
				{
					((S5Cycle)cycle).ChainCycle = await _anodeUOW.StationCycle.GetByWithThrow(
						[cycle => cycle.RID == cycleRID]) as S3S4Cycle;
				}
			}

			_anodeUOW.Commit();
		}
		catch (EntityNotFoundException)
		{
		}
		catch (Exception)
		{
			throw;
		}

		await _anodeUOW.CommitTransaction();
	}

	private static KPI CreateKPI(IntPtr retMatch)
	{
		KPI kPI = new() {
			MScore = DLLVisionImport.fcx_matchRet_similarityScore(retMatch),
			NbCandidats = DLLVisionImport.fcx_matchRet_cardinality_after_brut_force(retMatch), //TODO change when new DLL
			Threshold = DLLVisionImport.fcx_matchRet_threshold(retMatch),
			NMminScore = DLLVisionImport.fcx_matchRet_worstScore(retMatch),
			// kPI.NMmaxScore = DLLVisionImport.fcx_matchRet_bestScore(retMatch); TODO allow when DLL works
			//kPI.NMAvg = DLLVisionImport.fcx_matchRet_mean(retMatch);
			//kPI.NMStdev = Math.Sqrt(DLLVisionImport.fcx_matchRet_variance(retMatch));
			ComputeTime = DLLVisionImport.fcx_matchRet_elapsed(retMatch),
		};

		for (int i = 0; i < 10; i++)
		{
			kPI.TenBestMatches.Add(new TenBestMatch() {
				Rank = i,
				//AnodeID = DLLVisionImport.fcx_matchRet_bestsIdx_anodeId(retMatch, i), TODO allow when DLL works
				//Score = DLLVisionImport.fcx_matchRet_bestsIdx_score(retMatch, i), TODO allow when DLL works
				KPI = kPI,
			}
		);
		}

		return kPI;
	}

	public async Task<bool> GoMatch(List<string> origins, int instanceMatchID, int delay)
	{
		try
		{
			List<IOTDevice> iotDevices = await _anodeUOW.IOTDevice
				.GetAll([device => device is ITApiStation], withTracking: false);
			ServerRule? rule = await _anodeUOW.IOTDevice.GetBy([device => device is ServerRule]) as ServerRule;

			List<int> stationIDs = origins.ConvertAll(Station.StationNameToID);
			DateTimeOffset? oldestToSign = (await _anodeUOW.ToSign
				.GetBy(
					[toSign => stationIDs.Contains(toSign.StationID)],
					orderBy: query => query.OrderBy(
						toSign => toSign.ShootingTS)))
				?.ShootingTS
				?? DateTimeOffset.Now;

			DateTimeOffset? oldestToLoad = (await _anodeUOW.ToLoad
				.GetBy(
					[toLoad => toLoad.InstanceMatchID == instanceMatchID],
					orderBy: query => query.OrderBy(
						toLoad => toLoad.ShootingTS)))
				?.ShootingTS
				?? DateTimeOffset.Now;

			DateTimeOffset? oldestStation = DateTimeOffset.Now;
			foreach (int stationID in stationIDs)
			{
				DateTimeOffset? newOldestStation
					= (iotDevices.Find(device => device.RID.EndsWith(stationID.ToString())) as ITApiStation)?.OldestTSShooting;

				if (newOldestStation is not null && newOldestStation < oldestStation)
					oldestStation = newOldestStation;
			}

			Console.WriteLine(
				"{0} {1} {2} {3} {4}",
				rule?.Reinit == false,
				!iotDevices.Select(device => device.IsConnected).Contains(false),
				ValidDelay(oldestStation, delay),
				ValidDelay(oldestToLoad, delay),
				ValidDelay(oldestToSign, delay));

			return rule?.Reinit == false
				&& !iotDevices.Select(device => device.IsConnected).Contains(false)
				&& ValidDelay(oldestStation, delay)
				&& ValidDelay(oldestToLoad, delay)
				&& ValidDelay(oldestToSign, delay);
		}
		catch (Exception)
		{
			throw;
		}
	}

	private static bool ValidDelay(DateTimeOffset? date, int delay)
		=> (date is not null) && ((DateTimeOffset)date).AddDays(delay) > DateTimeOffset.Now;

	public static async Task<List<int>> GetMatchInstance(string anodeType, int stationID, IAnodeUOW anodeUOW)
	{
		try
		{
			List<Match> matches = (await anodeUOW.IOTDevice
				.GetAll([device => device is Match], withTracking: false))
				.Cast<Match>()
				.Where(match => match.AnodeType == anodeType && match.StationID == stationID)
				.ToList();

			List<ToMatch> toMatches = await anodeUOW.ToMatch
				.GetAll(
					[toMatch => matches.Select(match => match.InstanceMatchID).Contains(toMatch.InstanceMatchID)],
					withTracking: false);

			List<int> lightestMatchInstances = [];

			foreach (IGrouping<string, Match> familyGroup in matches.GroupBy(match => match.Family))
			{
				int lightestMatchInstance = familyGroup.First().InstanceMatchID;
				int lightestMatchInstanceCount = toMatches.Count(match => match.InstanceMatchID == lightestMatchInstance);
				foreach (Match match in familyGroup)
				{
					int instanceCount = toMatches.Count(toMatch => toMatch.InstanceMatchID == match.InstanceMatchID);
					if (instanceCount < lightestMatchInstanceCount)
					{
						lightestMatchInstance = match.InstanceMatchID;
						lightestMatchInstanceCount = instanceCount;
					}
				}

				lightestMatchInstances.Add(lightestMatchInstance);
			}

			return lightestMatchInstances;
		}
		catch (Exception)
		{
			throw;
		}
	}
}