using Core.Entities.BI.BITemperatures.Models.DTO;
using Core.Shared.Models.DB.Kernel;
using Core.Shared.Models.DB.Kernel.Interfaces;

namespace Core.Entities.BI.BITemperatures.Models.DB;

public partial class BITemperature : BaseEntity, IBaseEntity<BITemperature, DTOBITemperature>
{
	public int StationID { get; set; }
	public string CameraRID { get; set; }
	public double Temperature { get; set; }
}