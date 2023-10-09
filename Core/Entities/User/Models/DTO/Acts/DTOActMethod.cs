using Core.Entities.User.Models.DB.Acts;
using Core.Shared.Models.DTO.Kernel;
using Core.Shared.Models.DTO.Kernel.Interfaces;

namespace Core.Entities.User.Models.DTO.Acts;

public partial class DTOAct : DTOBaseEntity, IDTO<Act, DTOAct>
{
	public DTOAct()
	{
		RID = "";
	}

	public DTOAct(Act act)
	{
		ID = act.ID;
		RID = act.RID;
		EntityType = act.EntityType;
		ParentType = act.ParentType;
		TS = act.TS;
	}

	public override Act ToModel()
	{
		return new Act(this);
	}
}