using Core.Entities.User.Models.DB.Acts;

namespace Core.Entities.User.Models.DTO.Acts;

public partial class DTOAct
{
	public DTOAct()
	{
		RID = string.Empty;
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
		return new(this);
	}
}