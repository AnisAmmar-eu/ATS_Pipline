using Core.Entities.KPIData.KPIs.Models.DB;
using Core.Entities.StationCycles.Dictionaries;
using Core.Entities.StationCycles.Models.DB.MatchableCycles;
using Core.Shared.Extensions;

namespace Core.Entities.KPIData.KPIs.Models.DTO;

public partial class DTOStationKPI
{
	public DTOStationKPI(List<MatchableCycle> cycles)
	{
		this.AnodeCount = cycles.Count;
		this.AnodeRecognized = GetMatchedStationCycles(cycles).Count;

		List<KPI> kPIs = cycles.ConvertAll(cycle => cycle.KPI);

		this.RSizeAvg = AverageRSize(kPIs);
		this.RSizePeak = PeakRSize(kPIs);
		this.LastThreshold = LastThresholdScore(kPIs);

		this.NMScoreAvg = CalculateNMScoreAvg(kPIs);
		this.MScoreAvg = CalculateMScoreAvg(kPIs);

		this.IDRate = AnodeCount / AnodeRecognized;
		this.IDMean = MScoreAvg - NMScoreAvg;
		this.IDCluster = MScoreAvg - (3 * MScoreStdev) - (NMScoreAvg + (3 * NMScoreStdev));

		this.ComputeTimeAvg = ElapsedTimeAvg(kPIs);
	}

	public List<MatchableCycle> GetMatchedStationCycles(List<MatchableCycle> stationCycles)
	{
		return stationCycles
			.Where(cycle => cycle.MatchingCamera1 == SignMatchStatus.Ok || cycle.MatchingCamera2 == SignMatchStatus.Ok)
			.ToList();
	}

	public double AverageRSize(List<KPI> kpis) => kpis.Average(kpi => kpi.NbCandidats);

	public int PeakRSize(List<KPI> kpis) => kpis.Max(kpi => kpi.NbCandidats);

	public int LastThresholdScore(List<KPI> kpis) => kpis.OrderByDescending(kpi => kpi.TS).ElementAt(0).Threshold;

	public double CalculateMScoreAvg(List<KPI> kpis) => kpis.WeightedAverage(kpi => kpi.MScore, kpi => kpi.NbCandidats);
	public double CalculateNMScoreAvg(List<KPI> kpis) => kpis.WeightedAverage(kpi => kpi.NMAvg, kpi => kpi.NbCandidats);

	public double CalculateMScoreStdev(List<KPI> kpis) => kpis.Select(kpi =>(double) kpi.MScore).StandardDeviation();
	//public double CalculateNMScoreStdev(List<KPI> kpis) => kpis.Select(kpi =>(double) kpi.MScore).StandardDeviation();

	public double ElapsedTimeAvg(List<KPI> kpis) => kpis.Average(kpi => kpi.ComputeTime);
}