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
using Core.Shared.Exceptions;
using Core.Shared.Services.Kernel;
using Core.Shared.UnitOfWork.Interfaces;
using Core.Shared.DLLVision;
using Core.Entities.KPIData.WarningMsgs.Models.DB;
using System.Runtime.InteropServices;
using Serilog;

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

				((S5Cycle)cycle).ChainMatchingResult = true;
			}
			else
			{
				if (cameraID == 1)
					cycle.MatchingCamera1 = SignMatchStatus.Ok;

				if (cameraID == 2)
					cycle.MatchingCamera2 = SignMatchStatus.Ok;

				cycle.MatchingResult = true;
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
			cycle.KPI = CreateKPI(retMatch, retMatchCode);

		_anodeUOW.StationCycle.Update(cycle);
		_anodeUOW.Commit();

		await _anodeUOW.CommitTransaction();
		return cycle;
	}

	public async Task UpdateAnode(MatchableCycle cycle, string? cycleRID)
	{
		if (cycleRID is null)
			return;

		try
		{
			Anode anode = await _anodeUOW.Anode.GetByWithThrow(
				[anode => anode.CycleRID == cycleRID]
				);

			if (cycle is S3S4Cycle)
			{
				anode.S3S4Cycle = cycle as S3S4Cycle;
				anode.S3S4StationID = cycle.StationID;
				anode.S3S4SignStatus1 = cycle.SignStatus1;
				anode.S3S4SignStatus2 = cycle.SignStatus2;
				anode.SS3S4MatchingCamera1 = cycle.MatchingCamera1;
				anode.S3S4MatchingCamera2 = cycle.MatchingCamera2;
				anode.S3S4TSFirstShooting = cycle.TSFirstShooting;
				if (!anode.IsComplete)
					anode.IsComplete = cycle.AnodeType == AnodeTypeDict.D20;
			}
			else
			{
				((AnodeDX)anode).S5Cycle = cycle as S5Cycle;
				((AnodeDX)anode).S5StationID = cycle.StationID;
				((AnodeDX)anode).SSignStatus1 = cycle.SignStatus1;
				((AnodeDX)anode).S5SignStatus2 = cycle.SignStatus2;
				((AnodeDX)anode).S5MatchingCamera1 = cycle.MatchingCamera1;
				((AnodeDX)anode).S5MatchingCamera2 = cycle.MatchingCamera2;
				((AnodeDX)anode).S5TSFirstShooting = cycle.TSFirstShooting;
				anode.IsComplete = cycle.AnodeType == AnodeTypeDict.DX;
			}

			_anodeUOW.Anode.Update(anode);
			_anodeUOW.Commit();
		}
		catch (EntityNotFoundException)
		{
		}
		catch (Exception)
		{
			throw;
		}
	}

	public async Task UpdateChainedCycle(MatchableCycle cycle, string? cycleRID)
	{
		if (cycleRID is null)
			return;

		await _anodeUOW.StartTransaction();
		((S5Cycle)cycle).ChainCycle = (S3S4Cycle?)await _anodeUOW.StationCycle.GetByWithThrow(
			[cycle => cycle.RID == cycleRID]);
		_anodeUOW.StationCycle.Update(cycle);

		_anodeUOW.Commit();
		await _anodeUOW.CommitTransaction();
	}

	private static KPI CreateKPI(IntPtr retMatch, int retMatchCode)
	{
		KPI kPI = new() {
			MScore = DLLVisionImport.fcx_matchRet_similarityScore(retMatch),
			NbCandidats = DLLVisionImport.fcx_matchRet_cardinality_after_brut_force(retMatch),
			Threshold = DLLVisionImport.fcx_matchRet_threshold(retMatch),
			//NMminScore = DLLVisionImport.fcx_matchRet_worstScore(retMatch),
			//NMmaxScore = DLLVisionImport.fcx_matchRet_bestScore(retMatch),
			//NMAvg = DLLVisionImport.fcx_matchRet_mean(retMatch),
			//NMStdev = Math.Sqrt(DLLVisionImport.fcx_matchRet_variance(retMatch)),
			ComputeTime = DLLVisionImport.fcx_matchRet_elapsed(retMatch),
		};

		for (int i = 0; i < Math.Min(10, kPI.NbCandidats); i++)
		{
			kPI.TenBestMatches.Add(new TenBestMatch() {
				Rank = i,
				//AnodeID = Marshal.PtrToStringAnsi(DLLVisionImport.fcx_matchRet_bestsIdx_anodeId(retMatch, i)) ?? string.Empty,
				//Score = DLLVisionImport.fcx_matchRet_bestsIdx_score(retMatch, i),
				KPI = kPI,
			}
		);
		}

		if (retMatchCode == -106)
		{
			for (int i = 0; i < DLLVisionImport.fcx_warnings_nb_messages(); i++)
			{
				nint warningMsgPtr = DLLVisionImport.fcx_warnings_get_message(i);
				string? warningMsg = Marshal.PtrToStringAnsi(warningMsgPtr);
				if (warningMsg is null)
					continue;

				string[] splitMsg = warningMsg.Split(";", 2);

				kPI.WarningMsgs.Add(new WarningMsg() {
					Code = int.Parse(splitMsg[0]),
					Message = splitMsg[1],
					KPI = kPI,
				}
			);
			}
		}

		return kPI;
	}

	public async Task<bool> GoMatch(List<string> origins, int instanceMatchID, double delay, DateTimeOffset? oldestToMatch)
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
				?? DateTimeOffset.MinValue;

			DateTimeOffset? oldestToLoad = (await _anodeUOW.ToLoad
				.GetBy(
					[toLoad => toLoad.InstanceMatchID == instanceMatchID],
					orderBy: query => query.OrderBy(
						toLoad => toLoad.ShootingTS)))
				?.ShootingTS
				?? DateTimeOffset.MinValue;

			DateTimeOffset? oldestStation = DateTimeOffset.Now;
			foreach (int stationID in stationIDs)
			{
				DateTimeOffset? newOldestStation
					= (iotDevices.Find(device => device.RID.EndsWith(stationID.ToString())) as ITApiStation)?.OldestTSShooting;

				if (newOldestStation is not null && newOldestStation < oldestStation)
					oldestStation = newOldestStation;
			}

			Log.Information(
				"{0} {1} {2} {3} {4} {5} {6} {7}",
				rule?.Reinit == false,
				!iotDevices.Select(device => device.IsConnected).Contains(false),
				ValidDelay(oldestStation, delay),
				ValidDelay(oldestToLoad, delay),
				ValidDelay(oldestToSign, delay),
				((DateTimeOffset)oldestToLoad).AddDays(delay) < oldestToMatch,
				((DateTimeOffset)oldestToSign).AddDays(delay) < oldestToMatch,
				!await _anodeUOW.ToUnload.AnyPredicate(toUnload => toUnload.InstanceMatchID == instanceMatchID));

			return rule?.Reinit == false
				&& !iotDevices.Select(device => device.IsConnected).Contains(false)
				&& ValidDelay(oldestStation, delay)
				&& ValidDelay(oldestToLoad, delay)
				&& ValidDelay(oldestToSign, delay)
				&& ((DateTimeOffset)oldestToLoad).AddDays(delay) < oldestToMatch
				&& ((DateTimeOffset)oldestToSign).AddDays(delay) < oldestToMatch
				&& !await _anodeUOW.ToUnload.AnyPredicate(toUnload => toUnload.InstanceMatchID == instanceMatchID);
		}
		catch (Exception)
		{
			throw;
		}
	}

	private static bool ValidDelay(DateTimeOffset? date, double delay)
		=> (date is not null) && ((DateTimeOffset)date).AddDays(delay) < DateTimeOffset.Now;

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