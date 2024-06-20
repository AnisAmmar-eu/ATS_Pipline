using Core.Entities.KPIData.KPIs.Models.DTO;
using Core.Shared.Services.Kernel;
using Core.Shared.UnitOfWork.Interfaces;
using Core.Entities.KPIData.KPIs.Models.DB;
using Core.Entities.KPIData.KPIs.Repositories;
using Core.Entities.StationCycles.Models.DB.MatchableCycles;
using Core.Shared.Dictionaries;

namespace Core.Entities.KPIData.KPIs.Services;

public class KPIService :
	BaseEntityService<IKPIRepository, KPI, DTOKPI>,
	IKPIService
{
	public KPIService(IAnodeUOW anodeUOW) : base(anodeUOW)
	{
	}

	public async Task<List<DTOStationKPI>> CreateAllStationKPIByPeriod(
		DateTimeOffset? start,
		DateTimeOffset? end,
		List<string> anodeTypes,
		List<string> stationOrigin)
	{
		List<DTOStationKPI> dTOStationKPIs = [];
		List<MatchableCycle> cycles = (await _anodeUOW.StationCycle
			.GetAll(
				[cycle => cycle is MatchableCycle && cycle.TS >= start && cycle.TS <= end && anodeTypes.Contains(cycle.AnodeType)],
				includes: [nameof(MatchableCycle.KPI), nameof(MatchableCycle.Anode)]))
			.Cast<MatchableCycle>()
			.ToList();

		Console.WriteLine($"Cycles récupérés : {cycles.Count.ToString()}");
		List<int> stationIDs = Station.Stations.ConvertAll(Station.StationNameToID);
		Console.WriteLine($"Identifiants des stations : {string.Join(", ", stationIDs)}");

		foreach (int stationID in stationIDs)
		{
			List<MatchableCycle> cyclesToKPI = cycles.Where(cycle => cycle.StationID == stationID).ToList();
			Console.WriteLine($"StationID : {stationID.ToString()}, Nombre de cycles pour KPI : {cyclesToKPI.Count.ToString()}");

			if (!Station.IsMatchStation(stationID) || cyclesToKPI.Count == 0)
				continue;

			dTOStationKPIs.Add(new DTOStationKPI(cyclesToKPI, stationOrigin));
		}

		Console.WriteLine($"Nombre total de DTOStationKPIs  : {dTOStationKPIs.Count.ToString()}");
		return dTOStationKPIs;
	}
}