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

		List<KPI> kpis = cycles.ConvertAll(cycle => cycle.KPI);

		this.RSizeAvg = AverageRSize(kpis);
		this.RSizePeak = PeakRSize(kpis);
		this.LastThreshold = LastThresholdScore(kpis);

		this.NMScoreAvg = CalculateNMScoreAvg(kpis);
		this.MScoreAvg = CalculateMScoreAvg(kpis);
		//manque appel
		this.MScoreStdev = CalculateMScoreStdev(kpis);
		this.NMScoreStdev = CalculateNMScoreStdev(kpis);

		this.IDRate = (AnodeRecognized == 0) ? 0 : AnodeCount / AnodeRecognized;
		this.IDMean = MScoreAvg - NMScoreAvg;
		this.IDCluster = MScoreAvg - (3 * MScoreStdev) - (NMScoreAvg + (3 * NMScoreStdev));

		this.ComputeTimeAvg = ElapsedTimeAvg(kpis);
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