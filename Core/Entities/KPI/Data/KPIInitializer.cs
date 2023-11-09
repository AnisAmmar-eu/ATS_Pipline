using Core.Entities.KPI.KPICs.Dictionaries;
using Core.Entities.KPI.KPICs.Models.DB;
using Core.Shared.Data;

namespace Core.Entities.KPI.Data;

public class KPIInitializer
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
		anodeCTX.SaveChanges();

		for (int i = 1; i <= 5; ++i)
			InitializeStation(anodeCTX, i);
	}

	public static void InitializeStation(AnodeCTX anodeCTX, int stationID)
	{
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
		anodeCTX.SaveChanges();
	}
}