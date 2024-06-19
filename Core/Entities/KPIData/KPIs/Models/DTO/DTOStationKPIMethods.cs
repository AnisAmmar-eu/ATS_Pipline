using Core.Entities.KPIData.KPIs.Models.DB;
using Core.Entities.StationCycles.Models.DB.MatchableCycles;
using Core.Entities.StationCycles.Models.DB.MatchableCycles.S5Cycles;
using Core.Shared.Extensions;

namespace Core.Entities.KPIData.KPIs.Models.DTO;

public partial class DTOStationKPI
{
	public DTOStationKPI(List<MatchableCycle> cycles, List<string> stationOrigin)
	{
		this.AnodeCount = cycles.Count;
		this.AnodeRecognized = GetMatchedStationCycles(cycles, stationOrigin);

		List<KPI> kpis = cycles.Where(cycle => cycle.KPI is not null).Select(cycle => cycle.KPI!).ToList();

		this.RSizeAvg = AverageRSize(kpis);
		this.RSizePeak = PeakRSize(kpis);
		this.LastThreshold = LastThresholdScore(kpis);

		this.NMScoreAvg = CalculateNMScoreAvg(kpis);
		this.MScoreAvg = CalculateMScoreAvg(kpis);
		this.MScoreStdev = CalculateMScoreStdev(kpis);
		this.NMScoreStdev = CalculateNMScoreStdev(kpis);

		this.IDRate = (AnodeCount == 0) ? 0 : (double)AnodeRecognized / AnodeCount * 100;
		this.IDMean = MScoreAvg - NMScoreAvg;
		this.IDCluster = MScoreAvg - (3 * MScoreStdev) - (NMScoreAvg + (3 * NMScoreStdev));

		this.ComputeTimeAvg = ElapsedTimeAvg(kpis);
	}

	public int GetMatchedStationCycles(List<MatchableCycle> stationCycles, List<string> stationOrigin)
	{
		if (stationCycles[0] is S5Cycle)
		{
			List<S5Cycle?> cycles = stationCycles.ConvertAll(stationCycles => stationCycles as S5Cycle);
			return cycles.Count(cycle => cycle.Anode is not null
				&& stationOrigin.Contains($"S{cycle.Anode.CycleRID[0].ToString()}"));
		}
		else
		{
			return stationCycles.Count(cycle => cycle.Anode is not null
				&& stationOrigin.Contains($"S{cycle.Anode.CycleRID[0].ToString()}"));
		}
	}

	public double AverageRSize(List<KPI> kpis) => kpis.Average(kpi => kpi.NbCandidats);

	public int PeakRSize(List<KPI> kpis) => kpis.Max(kpi => kpi.NbCandidats);

	public int LastThresholdScore(List<KPI> kpis) => kpis.OrderByDescending(kpi => kpi.TS).ElementAt(0).Threshold;

	public double CalculateMScoreAvg(List<KPI> kpis) => kpis.WeightedAverage(kpi => kpi.MScore, kpi => kpi.NbCandidats);
	public double CalculateNMScoreAvg(List<KPI> kpis) => kpis.WeightedAverage(kpi => kpi.NMAvg, kpi => kpi.NbCandidats);

	public double CalculateMScoreStdev(List<KPI> kpis) => kpis.Select(kpi => (double)kpi.MScore).StandardDeviation();

	public double CalculateNMScoreStdev(List<KPI> kpis)
	{
		if (kpis.Count <= 1)
		{
			Console.WriteLine("Not enough data points. Returning 0.");
			return 0;
		}

		double avgNMScore = CalculateNMScoreAvg(kpis);
		double sumNbCandidats = kpis.Sum(kpi => kpi.NbCandidats);
		double sumVariance = kpis.Sum(kpi => kpi.NbCandidats * Math.Pow(kpi.NMAvg - avgNMScore, 2));

		if (sumNbCandidats == 0)
			return 0;

		double variance = sumVariance / sumNbCandidats;
		return Math.Sqrt(variance);
	}

	public double ElapsedTimeAvg(List<KPI> kpis) => kpis.Average(kpi => kpi.ComputeTime);
}