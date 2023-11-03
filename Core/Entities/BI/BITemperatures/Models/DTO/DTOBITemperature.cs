using Core.Entities.BI.BITemperatures.Models.DB;
using Core.Shared.Models.DTO.Kernel;
using Core.Shared.Models.DTO.Kernel.Interfaces;

namespace Core.Entities.BI.BITemperatures.Models.DTO;

public partial class DTOBITemperature : DTOBaseEntity, IDTO<BITemperature, DTOBITemperature>
{
	public int StationID { get; set; }
	public string CameraRID { get; set; } = string.Empty;
	public double Temperature { get; set; }
}