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
using Core.Entities.StationCycles.Models.DB.MatchableCycles.S5Cycles;
using Core.Entities.KPIData.KPIs.Models.DB;
using DLLVision;
using Core.Entities.KPIData.TenBestMatchs.Models.DB;
using Core.Entities.IOT.IOTDevices.Models.DB.ITApiStations;
using Core.Entities.IOT.IOTDevices.Models.DB.ServerRules;
using Core.Entities.IOT.IOTDevices.Models.DB;
using System.Reactive.Linq;
using Core.Shared.Dictionaries;
using System.Data;
using Core.Entities.IOT.IOTDevices.Models.DB.BackgroundServices.Matchs;

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
		await AnodeUOW.StartTransaction();

		if (DLLVisionImport.fcx_matchRet_errorCode(retMatch) == 0)
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
					cycle.MatchingCamera1 = SignMatchStatus.NotOk;
			}
		}

		cycle.KPI = CreateKPI(retMatch);

		_ = AnodeUOW.StationCycle.Update(cycle);
		_ = AnodeUOW.Commit();

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

			if (cycle is StationCycles.Models.DB.MatchableCycles.S3S4Cycles.S3S4Cycle)
				anode.S3S4Cycle = cycle as StationCycles.Models.DB.MatchableCycles.S3S4Cycles.S3S4Cycle;
			else
				((AnodeDX)anode).S5Cycle = cycle as S5Cycle;

			_ = AnodeUOW.Commit();
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

	private static KPI CreateKPI(IntPtr retMatch)
	{
		KPI kPI = new()
		{
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
			kPI.TenBestMatches.Add(new TenBestMatch()
				{
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
			List<IOTDevice> iotDevices = await AnodeUOW.IOTDevice
				.GetAll([device => device is ITApiStation], withTracking: false);
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
					[toLoad => toLoad.InstanceMatchID == instanceMatchID],
					orderBy: query => query.OrderByDescending(
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
				ValidDelay(oldestStation, delay));

			return rule?.Reinit == false
				&& !iotDevices.Select(device => device.IsConnected).Contains(false)
				&& ValidDelay(oldestStation, delay)
				&& ValidDelay(oldestToLoad, delay)
				&& ValidDelay(oldestStation, delay);
		}
		catch (Exception)
		{
			throw;
		}
	}

	private static bool ValidDelay(DateTimeOffset? date, int delay)
		=> (date is not null) && ((DateTimeOffset)date).AddDays(delay) > DateTimeOffset.Now;

	public static async Task<int> GetMatchInstance(string anodeType, int stationID, IAnodeUOW anodeUOW)
	{
		try
		{
			List<int> matchInstances = (await anodeUOW.IOTDevice
				.GetAll([device => device is Match], withTracking: false))
				.Cast<Match>()
				.Where(match => match.AnodeType == anodeType && match.StationID == stationID)
				.Select(match => match.InstanceMatchID)
				.ToList();

			if (matchInstances.Count == 1)
				return matchInstances[0];

			List<ToMatch> toMatches = await anodeUOW.ToMatch
				.GetAll([toMatch => matchInstances.Contains(toMatch.InstanceMatchID)], withTracking: false);

			int lightestMatchInstance = matchInstances[0];
			int lightestMatchInstanceCount = toMatches.Count(match => match.InstanceMatchID == lightestMatchInstance);
			foreach (int matchInstance in matchInstances)
			{
				int instanceCount = toMatches.Count(match => match.InstanceMatchID == matchInstance);
				if (instanceCount < lightestMatchInstanceCount)
				{
					lightestMatchInstance = matchInstance;
					lightestMatchInstanceCount = instanceCount;
				}
			}

			return lightestMatchInstance;
		}
		catch (Exception)
		{
			throw;
		}
	}
}