using Core.Entities.KPI.KPICs.Dictionaries;
using Core.Entities.KPI.KPICs.Models.DB;
using Core.Migrations;
using Core.Shared.Data;

namespace Core.Entities.KPI.Data;

public static class KPIInitializer
{
	public static void Initialize(AnodeCTX anodeCTX)
	{
		if (anodeCTX.KPIC.Any())
			return;

		// == Station cycles for sign / match graphs ==
		anodeCTX.KPIC.Add(new KPIC(KPICRID.StationCycleMatched, "Station cycle matched",
			"Number of station cycles which matched."));
		anodeCTX.KPIC.Add(new KPIC(KPICRID.StationCycleSigned, "Station cycle signed and not matched",
			"Number of station cycles with both signed and not matched."));
		anodeCTX.KPIC.Add(new KPIC(KPICRID.StationCycleNotSigned, "Station cycle not signed",
			"Number of station cycles where not all pictures are signed."));
		anodeCTX.KPIC.Add(new KPIC(KPICRID.StationCycleMatchingCam1, "Station cycle matching camera 1",
			"Percentage of station cycles matched by camera 1."));

		// == Anode count by stations and type ==
		anodeCTX.KPIC.Add(new KPIC(KPICRID.AnodesTotalNumber, "Anodes total number", "Total number of anodes"));
		anodeCTX.KPIC.Add(new KPIC(KPICRID.D20Anodes, "D20 Anodes", "Total number of D20 Anodes"));
		anodeCTX.KPIC.Add(new KPIC(KPICRID.DXAnodes, "DX Anodes", "Total number of DX Anodes"));

		anodeCTX.SaveChanges();
		for (int i = 1; i <= 5; ++i)
			InitializeStationForServer(anodeCTX, i);
	}

	private static void InitializeStationForServer(AnodeCTX anodeCTX, int stationID)
	{
		// == Station cycles for sign / match graphs ==
		anodeCTX.KPIC.Add(new KPIC(KPICRID.StationCycleMatched + stationID,
			"Station cycle matched for station" + stationID,
			"Number of station cycles which matched for station" + stationID));
		anodeCTX.KPIC.Add(new KPIC(KPICRID.StationCycleSigned + stationID,
			"Station cycle signed and not matched for station" + stationID,
			"Number of station cycles with both signed and not matched for station" + stationID));
		anodeCTX.KPIC.Add(new KPIC(KPICRID.StationCycleNotSigned + stationID,
			"Station cycle not signed for station" + stationID,
			"Number of station cycles where not all pictures are signed for station" + stationID));
		anodeCTX.KPIC.Add(new KPIC(KPICRID.StationCycleMatchingCam1 + stationID,
			"Station cycle matching camera 1 for station" + stationID,
			"Percentage of station cycles matched by camera 1 for station " + stationID));

		// == Anode count by stations and type ==
		anodeCTX.KPIC.Add(new KPIC(KPICRID.AnodesStation + stationID, "Number of anodes for station" + stationID,
			"Number of anodes which went through station" + stationID));
		anodeCTX.SaveChanges();
	}
}