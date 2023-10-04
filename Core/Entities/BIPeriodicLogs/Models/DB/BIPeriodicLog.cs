using Core.Entities.BIPeriodicLogs.Models.DTO;
using Core.Shared.Models.DB.Kernel;
using Core.Shared.Models.DB.Kernel.Interfaces;

namespace Core.Entities.BIPeriodicLogs.Models.DB;

// BI = Business Intelligence
public partial class BIPeriodicLog : BaseEntity, IBaseEntity<BIPeriodicLog, DTOBIPeriodicLog>
{
	public int NbMatched { get; set; }
	public int NbSigned { get; set; }
	public int NbUnsigned { get; set; }

	public int Cam1Matched { get; set; }
	public int Cam2Matched { get; set; }

	public int InactiveAlarms { get; set; }
	public int NonAckAlarms { get; set; }
	public int ActiveAlarms { get; set; }

	public int NbAnodeS1 { get; set; }
	public int NbAnodeS2 { get; set; }
	public int NbAnodeS3 { get; set; }
	public int NbAnodeS4 { get; set; }
	public int NbAnodeS5 { get; set; }
}