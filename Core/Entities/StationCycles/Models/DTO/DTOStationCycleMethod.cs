using Core.Entities.KPI.KPICs.Dictionaries;
using Core.Entities.KPI.KPICs.Models.DB;
using Core.Entities.StationCycles.Dictionaries;
using Core.Entities.StationCycles.Interfaces;
using Core.Entities.StationCycles.Models.DB;
using Core.Shared.Dictionaries;
using Core.Shared.Models.DB.Kernel.Interfaces;
using Core.Shared.Models.DTO.Kernel;
using Core.Shared.Models.DTO.Kernel.Interfaces;

namespace Core.Entities.StationCycles.Models.DTO;

public partial class DTOStationCycle : DTOBaseEntity, IDTO<StationCycle, DTOStationCycle>, IBaseKPI<DTOStationCycle>
{
	public DTOStationCycle()
	{
	}

	public DTOStationCycle(StationCycle stationCycle)
	{
		ID = stationCycle.ID;
		TS = stationCycle.TS;

		StationID = stationCycle.StationID;
		AnodeType = stationCycle.AnodeType;
		RID = stationCycle.RID;
		Status = stationCycle.Status;
		TSClosed = stationCycle.TSClosed;
		SignStatus1 = stationCycle.SignStatus1;
		SignStatus2 = stationCycle.SignStatus2;

		AnnouncementStatus = stationCycle.AnnouncementStatus;
		AnnouncementID = stationCycle.AnnouncementID;
		AnnouncementPacket = stationCycle.AnnouncementPacket?.ToDTO();

		DetectionStatus = stationCycle.DetectionStatus;
		DetectionID = stationCycle.DetectionID;
		DetectionPacket = stationCycle.DetectionPacket?.ToDTO();

		ShootingStatus = stationCycle.ShootingStatus;
		ShootingID = stationCycle.ShootingID;
		ShootingPacket = stationCycle.ShootingPacket?.ToDTO();

		AlarmListStatus = stationCycle.AlarmListStatus;
		AlarmListID = stationCycle.AlarmListID;
		AlarmListPacket = stationCycle.AlarmListPacket?.ToDTO();
	}

	public DTOStationCycle GetValue()
	{
		return this;
	}

	public string[] GetKPICRID()
	{
		List<string> signMatchRIDs = new()
		{
			KPICRID.StationCycleMatched, KPICRID.StationCycleSigned, KPICRID.StationCycleNotSigned,
			KPICRID.StationCycleMatchingCam1
		};
		List<string> ans = new();
		// RIDS of the server and every station are added in the following order: rids, rids1, rids2...
		for (int i = 0; i <= 5; ++i)
			ans.AddRange(signMatchRIDs.Select(rid => rid + (i == 0 ? "" : i.ToString())));
		
		ans.AddRange(new[] { KPICRID.D20Anodes, KPICRID.DXAnodes });
		for (int i = 1; i <= 5; ++i)
			ans.Add($"{KPICRID.AnodesStation}{i}");

		return ans.ToArray();
	}

	public Func<List<DTOStationCycle>, string[]> GetComputedValues()
	{
		return stationCycles =>
		{
			const int nbSignedAndMatchedIndex = 0;
			const int nbSignedIndex = 1;
			const int nbNotSignedIndex = 2;
			const int nbMatchCam1Index = 3;
			const int nbTotalMatchIndex = 4;
			int nbD20 = 0;
			int nbDX = 0;
			// Because there is 6 times 5 values
			int[,] signMatchValues = new int[6, 5];
			int[] nbAnodes = new int[5];
			stationCycles.ForEach(cycle =>
			{
				nbAnodes[cycle.StationID - 1]++;
				if (cycle.AnodeType == AnodeTypeDict.D20) nbD20++;
				else if (cycle.AnodeType == AnodeTypeDict.DX) nbDX++;
				if (cycle.SignStatus1 == SignMatchStatus.Ok || cycle.SignStatus2 == SignMatchStatus.Ok)
				{
					if (cycle is IMatchableCycle matchableCycle &&
					    (matchableCycle.MatchingCamera1 == SignMatchStatus.Ok ||
					     matchableCycle.MatchingCamera2 == SignMatchStatus.Ok))
					{
						AddAtIndex(signMatchValues, cycle.StationID, nbSignedAndMatchedIndex);
						AddAtIndex(signMatchValues, cycle.StationID, nbTotalMatchIndex);
						if (matchableCycle.MatchingCamera1 == SignMatchStatus.Ok)
							AddAtIndex(signMatchValues, cycle.StationID, nbMatchCam1Index);
					}
					else
					{
						AddAtIndex(signMatchValues, cycle.StationID, nbSignedIndex);
					}
				}
				else
				{
					AddAtIndex(signMatchValues, cycle.StationID, nbNotSignedIndex);
				}
			});
			List<string> ans = new();
			// The int[,] is flattened into a List<string> and the match cam1 percentage is computed.
			for (int i = 0; i < signMatchValues.GetLength(0); ++i)
			{
				for (int j = 0; j < 3; ++j)
					ans.Add(signMatchValues[i, j].ToString());
				int nbMatchCam1 = signMatchValues[i, nbMatchCam1Index];
				int nbTotalMatch = signMatchValues[i, nbTotalMatchIndex];
				int percentageCam1 = nbTotalMatch == 0 ? 0 : (int)((double)nbMatchCam1 / nbTotalMatch * 100);
				ans.Add(percentageCam1.ToString());
			}
			ans.Add(nbD20.ToString());
			ans.Add(nbDX.ToString());
			ans.AddRange(nbAnodes.Select(x => x.ToString()));

			return ans.ToArray();
		};
	}

	public override StationCycle ToModel()
	{
		return new StationCycle(this);
	}

	private static void AddAtIndex(int[,] table, int stationID, int index)
	{
		// 0 is the index of the global values.
		table[0, index]++;
		table[stationID, index]++;
	}
}