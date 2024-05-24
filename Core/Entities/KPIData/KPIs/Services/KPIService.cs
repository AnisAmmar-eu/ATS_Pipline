using Core.Entities.KPIData.KPIs.Models.DTO;
using Core.Shared.Services.Kernel;
using Core.Shared.UnitOfWork.Interfaces;
using Core.Entities.KPIData.KPIs.Models.DB;
using Core.Entities.KPIData.KPIs.Repositories;
using Core.Entities.StationCycles.Models.DB.MatchableCycles;
using Core.Shared.Dictionaries;
using Core.Entities.StationCycles.Models.DB;

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
			.Where(cycle => cycle.Anode is not null && stationOrigin.Contains($"S{cycle.Anode.CycleRID[0].ToString()}"))
			.ToList();
		foreach (int stationID in Station.Stations.ConvertAll(Station.StationNameToID))
		{
			List<MatchableCycle> cyclesToKPI = cycles.Where(cycle => cycle.StationID == stationID).ToList();
			if (!Station.IsMatchStation(stationID) || cyclesToKPI.Count == 0)
				continue;

			dTOStationKPIs.Add(new DTOStationKPI(cyclesToKPI));
		}

		return dTOStationKPIs;
	}
}