using Core.Entities.BIPeriodicLogs.Models.DB.RT.AnnualRTs;
using Core.Shared.Models.DTO.Kernel.Interfaces;

namespace Core.Entities.BIPeriodicLogs.Models.DTO.RT.AnnualRTs;

public partial class DTOAnnualRT : DTOBIPeriodicLog, IDTO<AnnualRT, DTOAnnualRT>
{
}