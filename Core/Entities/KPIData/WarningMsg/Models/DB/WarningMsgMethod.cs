using Core.Entities.KPIData.WarningMsgs.Models.DTO;
using Mapster;

namespace Core.Entities.KPIData.WarningMsgs.Models.DB;

public partial class WarningMsg
{
	public WarningMsg()
	{
	}

	public override DTOWarningMsg ToDTO() => this.Adapt<DTOWarningMsg>();
}