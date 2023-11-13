using Core.Entities.User.Dictionaries;
using Core.Entities.User.Models.DB.Roles;
using Core.Entities.User.Models.DB.Users;
using Core.Entities.User.Models.DTO.Acts.ActEntities.ActEntityRoles;

namespace Core.Entities.User.Models.DB.Acts.ActEntities.ActEntityRoles;

public partial class ActEntityRole
{
	public ActEntityRole()
	{
		RID = "";
		ApplicationID = "";
		ApplicationType = ApplicationTypeRID.USER;
	}

	public ActEntityRole(ActEntity actEntity, ApplicationUser user)
	{
		RID = actEntity.RID + "." + actEntity.ID;
		ActEntity = actEntity;
		ApplicationType = ApplicationTypeRID.USER;
		ApplicationID = user.Id;
	}

	public ActEntityRole(ActEntity actEntity, ApplicationRole role)
	{
		RID = actEntity.RID + "." + actEntity.ID;
		ActEntity = actEntity;
		ApplicationType = ApplicationTypeRID.ROLE;
		ApplicationID = role.Id;
	}

	public new DTOActEntityRole ToDTO()
	{
		return new DTOActEntityRole(this);
	}

	public DTOActEntityRole ToDTO(string? applicationName)
	{
		return new DTOActEntityRole(this, applicationName);
	}
}