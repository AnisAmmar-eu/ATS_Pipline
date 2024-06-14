using Core.Entities.KPIData.WarningMsgs.Models.DB;
using Core.Entities.KPIData.WarningMsgs.Models.DTO;
using Core.Entities.KPIData.WarningMsgs.Repositories;
using Core.Shared.Services.Kernel;
using Core.Shared.UnitOfWork.Interfaces;

namespace Core.Entities.KPIData.WarningMsgs.Services;

public class WarningMsgService :
	BaseEntityService<IWarningMsgRepository, WarningMsg, DTOWarningMsg>,
	IWarningMsgService
{
	public WarningMsgService(IAnodeUOW anodeUOW) : base(anodeUOW)
	{
	}
}