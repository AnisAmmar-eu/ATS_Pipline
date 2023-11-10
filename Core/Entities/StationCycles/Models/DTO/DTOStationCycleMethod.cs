using Core.Entities.KPI.KPICs.Dictionaries;
using Core.Entities.StationCycles.Dictionaries;
using Core.Entities.StationCycles.Interfaces;
using Core.Entities.StationCycles.Models.DB;
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
		List<string> rids = new()
		{
			KPICRID.StationCycleMatched, KPICRID.StationCycleSigned, KPICRID.StationCycleNotSigned,
			KPICRID.StationCycleMatchingCam1
		};
		List<string> ans = new();
		// RIDS of the server and every station are added in the following order: rids, rids1, rids2...
		for (int i = 0; i <= 5; ++i)
			ans.AddRange(rids.Select(rid => rid + (i == 0 ? "" : i.ToString())));

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
			// Because there is 6 times 5 values
			int[,] values = new int[6, 5];
			stationCycles.ForEach(cycle =>
			{
				if (cycle.SignStatus1 == SignMatchStatus.Ok || cycle.SignStatus2 == SignMatchStatus.Ok)
				{
					if (cycle is IMatchableCycle matchableCycle &&
					    (matchableCycle.MatchingCamera1 == SignMatchStatus.Ok ||
					     matchableCycle.MatchingCamera2 == SignMatchStatus.Ok))
					{
						AddAtIndex(values, cycle.StationID, nbSignedAndMatchedIndex);
						AddAtIndex(values, cycle.StationID, nbTotalMatchIndex);
						if (matchableCycle.MatchingCamera1 == SignMatchStatus.Ok)
							AddAtIndex(values, cycle.StationID, nbMatchCam1Index);
					}
					else
					{
						AddAtIndex(values, cycle.StationID, nbSignedIndex);
					}
				}
				else
				{
					AddAtIndex(values, cycle.StationID, nbNotSignedIndex);
				}
			});
			List<string> ans = new();
			// The int[,] is flattened into a List<string> and the match cam1 percentage is computed.
			for (int i = 0; i < values.GetLength(0); ++i)
			{
				for (int j = 0; j < 3; ++j)
					ans.Add(values[i, j].ToString());
				int nbMatchCam1 = values[i, nbMatchCam1Index];
				int nbTotalMatch = values[i, nbTotalMatchIndex];
				int percentageCam1 = nbTotalMatch == 0 ? 0 : (int)((double)nbMatchCam1 / nbTotalMatch * 100);
				ans.Add(percentageCam1.ToString());
			}

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