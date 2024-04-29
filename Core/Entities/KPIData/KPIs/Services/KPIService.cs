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
				[cycle => cycle is MatchableCycle && cycle.TS >= start && cycle.TS <= end && anodeTypes.Contains(cycle.AnodeType)]))
			.Cast<MatchableCycle>()
			.Where(cycle => cycle.Anode is null || stationOrigin.Contains("S" + cycle.Anode.CycleRID[0]))
			.ToList();

		foreach (int stationID in Station.Stations.ConvertAll(Station.StationNameToID))
		{
			if (!Station.IsMatchStation(stationID))
				continue;

			dTOStationKPIs.Add(new DTOStationKPI(cycles.Where(cycle => cycle.StationID == stationID).ToList()));
		}

		return dTOStationKPIs;
	}
}