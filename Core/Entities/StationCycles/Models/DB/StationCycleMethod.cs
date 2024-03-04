using System.Reflection;
using Core.Entities.KPI.KPICs.Dictionaries;
using Core.Entities.Packets.Models.DB;
using Core.Entities.StationCycles.Dictionaries;
using Core.Entities.StationCycles.Models.DB.LoadableCycles.S1S2Cycles;
using Core.Entities.StationCycles.Models.DB.MatchableCycles;
using Core.Entities.StationCycles.Models.DB.MatchableCycles.S3S4Cycles;
using Core.Entities.StationCycles.Models.DB.MatchableCycles.S5Cycles;
using Core.Entities.StationCycles.Models.DTO;
using Core.Entities.StationCycles.Models.Structs;
using Core.Shared.Dictionaries;

namespace Core.Entities.StationCycles.Models.DB;

public partial class StationCycle
{
	protected StationCycle()
	{
	}

	public StationCycle(DTOStationCycle dtoStationCycle)
	{
		AnodeType = dtoStationCycle.AnodeType;
		StationID = dtoStationCycle.StationID;
		RID = dtoStationCycle.RID;
		Status = dtoStationCycle.Status;
		TSClosed = dtoStationCycle.TSClosed;
		SignStatus1 = dtoStationCycle.SignStatus1;
		SignStatus2 = dtoStationCycle.SignStatus2;

		MetaDataID = dtoStationCycle.MetaDataID;
		MetaDataPacket = dtoStationCycle.MetaDataPacket?.ToModel();

		Picture1Status = dtoStationCycle.Picture1Status;
		Shooting1ID = dtoStationCycle.Shooting1ID;
		Shooting1Packet = dtoStationCycle.Shooting1Packet?.ToModel();

		Picture2Status = dtoStationCycle.Picture2Status;
		Shooting2ID = dtoStationCycle.Shooting2ID;
		Shooting2Packet = dtoStationCycle.Shooting2Packet?.ToModel();

		AlarmListID = dtoStationCycle.AlarmListID;
		AlarmListPacket = dtoStationCycle.AlarmListPacket?.ToModel();
	}

	public override DTOStationCycle ToDTO()
	{
		return new(this);
	}

	public StationCycle GetValue()
	{
		return this;
	}

	public void AssignPacket(Packet packet)
	{
		PropertyInfo? property = Array.Find(
			this.GetType()
				.GetProperties(BindingFlags.Instance | BindingFlags.Public),
			info => info.PropertyType == packet.GetType());
		if (property is null)
			throw new ArgumentException($"Cycle of RID {RID} has no packet of type {packet.GetType()}");

		property.SetValue(this, packet);
	}

	public static string[] GetKPICRID()
	{
		List<string> signMatchRIDs = [
			KPICRID.StationCycleMatched,
			KPICRID.StationCycleSigned,
			KPICRID.StationCycleNotSigned,
			KPICRID.StationCycleMatchingCam1,
		];
		List<string> ans = new();
		// RIDS of the server and every station are added in the following order: rids, rids1, rids2...
		for (int i = 0; i <= 5; ++i)
			ans.AddRange(signMatchRIDs.Select(rid => rid + ((i == 0) ? string.Empty : i.ToString())));

		ans.AddRange([KPICRID.D20Anodes, KPICRID.DXAnodes]);
		for (int i = 1; i <= 5; ++i)
			ans.Add($"{KPICRID.AnodesStation}{i.ToString()}");

		return ans.ToArray();
	}

	public Func<List<StationCycle>, string[]> GetComputedValues()
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
				if (cycle.AnodeType == AnodeTypeDict.D20)
					nbD20++;
				else if (cycle.AnodeType == AnodeTypeDict.DX)
					nbDX++;

				if (cycle.SignStatus1 == SignMatchStatus.Ok || cycle.SignStatus2 == SignMatchStatus.Ok)
				{
					if (cycle is MatchableCycle matchingCycle
						&& (matchingCycle.MatchingCamera1 == SignMatchStatus.Ok
							|| matchingCycle.MatchingCamera2 == SignMatchStatus.Ok))
					{
						AddAtIndex(signMatchValues, cycle.StationID, nbSignedAndMatchedIndex);
						AddAtIndex(signMatchValues, cycle.StationID, nbTotalMatchIndex);
						if (matchingCycle.MatchingCamera1 == SignMatchStatus.Ok)
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
				int percentageCam1 = (nbTotalMatch == 0) ? 0 : (int)((double)nbMatchCam1 / nbTotalMatch * 100);
				ans.Add(percentageCam1.ToString());
			}

			ans.Add(nbD20.ToString());
			ans.Add(nbDX.ToString());
			ans.AddRange(nbAnodes.Select(x => x.ToString()));

			return ans.ToArray();
		};
	}

	public static StationCycle Create(string stationName)
	{
		StationType stationType = Station.StationNameToType(stationName);
		return stationType switch {
			StationType.S1S2 => new S1S2Cycle(),
			StationType.S3S4 => new S3S4Cycle(),
			_ => new S5Cycle(),
		};
	}

	public DTOReducedStationCycle Reduce()
	{
		return new() {
			ID = ID,
			RID = RID,
			AnodeSize = null,
			AnodeType = AnodeType,
			ShootingTS = Shooting1Packet?.ShootingTS,
		};
	}

	private static void AddAtIndex(int[,] table, int stationID, int index)
	{
		// 0 is the index of the global values.
		table[0, index]++;
		table[stationID, index]++;
	}
}