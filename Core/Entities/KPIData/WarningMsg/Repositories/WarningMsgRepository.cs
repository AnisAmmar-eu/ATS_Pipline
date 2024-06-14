using Core.Entities.KPIData.WarningMsgs.Models.DB;
using Core.Entities.KPIData.WarningMsgs.Models.DTO;
using Core.Shared.Data;
using Core.Shared.Repositories.Kernel;

namespace Core.Entities.KPIData.WarningMsgs.Repositories;

public class WarningMsgRepository :
	BaseEntityRepository<AnodeCTX, WarningMsg, DTOWarningMsg>,
	IWarningMsgRepository
{
	public WarningMsgRepository(AnodeCTX context) : base(context, [], [])
	{
	}
}