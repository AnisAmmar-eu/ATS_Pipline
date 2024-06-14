using Core.Entities.KPIData.WarningMsgs.Models.DB;
using Core.Shared.Models.DTO.Kernel;
using Core.Shared.Models.DTO.Kernel.Interfaces;

namespace Core.Entities.KPIData.WarningMsgs.Models.DTO;

public partial class DTOWarningMsg : DTOBaseEntity, IDTO<WarningMsg, DTOWarningMsg>
{
	public int Code { get; set; }
	public string Message { get; set; }

	public int KPIID { get; set; }
}