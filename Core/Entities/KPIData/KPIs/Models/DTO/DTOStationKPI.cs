using Core.Shared.Models.DTO.Kernel;

namespace Core.Entities.KPIData.KPIs.Models.DTO;

public partial class DTOStationKPI : DTOBaseEntity
{
	public int StationID { get; set; }
	public int AnodeCount { get; set; }
	public int AnodeRecognized { get; set; }
	public double RSizeAvg { get; set; }
	public int RSizePeak { get; set; }
	public int LastThreshold { get; set; }

	public double NMScoreAvg { get; set; }
	public double NMScoreStdev { get; set; }

	public double MScoreAvg { get; set; }
	public double MScoreStdev { get; set; }

	public double IDRate { get; set; }
	public double IDMean { get; set; }
	public double IDCluster { get; set; }

	public double ComputeTimeAvg { get; set; }
}