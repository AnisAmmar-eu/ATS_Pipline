using Core.Entities.Vision.Dictionaries;
using Core.Entities.KPIData.KPIs.Models.DB;
using Core.Shared.Models.DTO.Kernel;
using Core.Shared.Models.DTO.Kernel.Interfaces;

namespace Core.Entities.KPIData.KPIs.Models.DTO;

public partial class DTOKPI : DTOBaseEntity, IDTO<KPI, DTOKPI>
{
	public int CycleID { get; set; }
	public string CycleRID { get; set; }
	public int CameraID { get; set; }
	public int StationID { get; set; }
	public string AnodeType { get; set; }
	public DateTimeOffset? ShootingTS { get; set; }
}