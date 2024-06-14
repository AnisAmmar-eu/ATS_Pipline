using Core.Entities.KPIData.WarningMsgs.Models.DB;
using Core.Entities.KPIData.WarningMsgs.Models.DTO;
using Core.Shared.Repositories.Kernel.Interfaces;

namespace Core.Entities.KPIData.WarningMsgs.Repositories;

public interface IWarningMsgRepository : IBaseEntityRepository<WarningMsg, DTOWarningMsg>
{
}