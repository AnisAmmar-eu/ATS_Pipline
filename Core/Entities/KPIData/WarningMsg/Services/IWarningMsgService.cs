using Core.Entities.KPIData.WarningMsgs.Models.DB;
using Core.Entities.KPIData.WarningMsgs.Models.DTO;
using Core.Shared.Services.Kernel.Interfaces;

namespace Core.Entities.KPIData.WarningMsgs.Services;

public interface IWarningMsgService : IBaseEntityService<WarningMsg, DTOWarningMsg>
{
}