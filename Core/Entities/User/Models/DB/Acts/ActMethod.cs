using Core.Entities.User.Models.DTO.Acts;

namespace Core.Entities.User.Models.DB.Acts;

public partial class Act
{
	public Act()
	{
		RID = "";
	}

	public Act(string rid, string? entityType = null, string? parentType = null)
	{
		RID = rid;
		EntityType = entityType;
		ParentType = parentType;
	}

	public Act(DTOAct dto)
	{
		RID = dto.RID;
		EntityType = dto.EntityType;
		ParentType = dto.ParentType;
	}

	public override DTOAct ToDTO()
	{
		return new DTOAct(this);
	}
}