using Core.Entities.KPIData.WarningMsgs.Models.DB;
using Mapster;

namespace Core.Entities.KPIData.WarningMsgs.Models.DTO;

public partial class DTOWarningMsg
{
	public DTOWarningMsg()
	{
	}

	public override WarningMsg ToModel() => this.Adapt<WarningMsg>();
}