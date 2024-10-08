using System.Reflection;
using Core.Entities.Packets.Models.DB;
using Core.Entities.StationCycles.Dictionaries;
using Core.Entities.StationCycles.Models.DB.LoadableCycles.S1S2Cycles;
using Core.Entities.StationCycles.Models.DB.MatchableCycles;
using Core.Entities.StationCycles.Models.DB.MatchableCycles.S3S4Cycles;
using Core.Entities.StationCycles.Models.DB.MatchableCycles.S5Cycles;
using Core.Entities.StationCycles.Models.DTO;
using Core.Entities.StationCycles.Models.Structs;
using Core.Shared.Dictionaries;
using Mapster;

namespace Core.Entities.StationCycles.Models.DB;

public partial class StationCycle
{
	protected StationCycle()
	{
	}

	public override DTOStationCycle ToDTO()
	{
		DTOStationCycle dtoStationCycle = this.Adapt<DTOStationCycle>();
		dtoStationCycle.MetaDataPacket = MetaDataPacket?.ToDTO();
		dtoStationCycle.Shooting1Packet = Shooting1Packet?.ToDTO();
		dtoStationCycle.Shooting2Packet = Shooting2Packet?.ToDTO();
		dtoStationCycle.AlarmListPacket = AlarmListPacket?.ToDTO();
		return dtoStationCycle;
	}

	/// <summary>
	/// Assigns a packet to the station cycle.
	/// </summary>
	/// <param name="packet">The packet to assign.</param>
	public void AssignPacket(Packet packet)
	{
		PropertyInfo[] properties = this.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
		Type packetType = packet.GetType();

		PropertyInfo property = Array.Find(properties, info => info.PropertyType == packetType)
			?? throw new ArgumentException($"Cycle of RID {RID} has no packet of type {packetType}");

		property.SetValue(this, packet);
	}

	public bool CanMatch()
	{
		return Station.IsMatchStation(StationID)
			&& !(Picture1Status == 0 && Picture2Status == 0)
			&& !(Picture1Status == 1
				&& (Shooting1ID is null
					|| SignStatus1 != SignMatchStatus.Ok))
			&& !(Picture2Status == 1
				&& (Shooting2ID is null
					|| SignStatus2 != SignMatchStatus.Ok));
	}

	public static Func<List<StationCycle>, string[]> GetComputedValues()
	{
		return stationCycles => {
			const int nbSignedAndMatchedIndex = 0;
			const int nbSignedIndex = 1;
			const int nbNotSignedIndex = 2;
			const int nbMatchCam1Index = 3;
			const int nbTotalMatchIndex = 4;
			int nbD20 = 0;
			int nbDX = 0;
			// Because there is 6 times 5 values
			int[][] signMatchValues = new int[6][];
			int[] nbAnodes = new int[5];
			stationCycles.ForEach(cycle => {
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
			List<string> ans = [];
			// The int[,] is flattened into a List<string> and the match cam1 percentage is computed.
			for (int i = 0; i < signMatchValues.GetLength(0); ++i)
			{
				for (int j = 0; j < 3; ++j)
					ans.Add(signMatchValues[i][j].ToString());

				int nbMatchCam1 = signMatchValues[i][nbMatchCam1Index];
				int nbTotalMatch = signMatchValues[i][nbTotalMatchIndex];
				int percentageCam1 = (nbTotalMatch == 0) ? 0 : (int)((double)nbMatchCam1 / nbTotalMatch * 100);
				ans.Add(percentageCam1.ToString());
			}

			ans.Add(nbD20.ToString());
			ans.Add(nbDX.ToString());
			ans.AddRange(nbAnodes.Select(x => x.ToString()));

			return [.. ans];
		};
	}

	public static StationCycle Create(string stationName)
	{
		StationType stationType = Station.StationNameToType(stationName);
		return stationType switch {
			StationType.S1S2 => new S1S2Cycle(),
			StationType.S3S4 => new S3S4Cycle(),
			StationType.S5 => new S5Cycle(),
			StationType.Server => throw new ArgumentException("Cannot create a server cycle"),
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

	private static void AddAtIndex(int[][] table, int stationID, int index)
	{
		// 0 is the index of the global values.
		table[0][index]++;
		table[stationID][index]++;
	}
}