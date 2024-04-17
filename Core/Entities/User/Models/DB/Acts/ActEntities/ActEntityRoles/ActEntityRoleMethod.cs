using Core.Entities.User.Dictionaries;
using Core.Entities.User.Models.DB.Roles;
using Core.Entities.User.Models.DB.Users;
using Core.Entities.User.Models.DTO.Acts.ActEntities.ActEntityRoles;

namespace Core.Entities.User.Models.DB.Acts.ActEntities.ActEntityRoles;

public partial class ActEntityRole
{
	public ActEntityRole()
	{
		RID = string.Empty;
		ApplicationID = string.Empty;
		ApplicationType = ApplicationTypeRID.User;
	}

	public ActEntityRole(ActEntity actEntity, ApplicationUser user)
	{
		RID = actEntity.RID + "." + actEntity.ID;
		ActEntity = actEntity;
		ApplicationType = ApplicationTypeRID.User;
		ApplicationID = user.Id;
	}

	public ActEntityRole(ActEntity actEntity, ApplicationRole role)
	{
		RID = actEntity.RID + "." + actEntity.ID;
		ActEntity = actEntity;
		ApplicationType = ApplicationTypeRID.Role;
		ApplicationID = role.Id;
	}

	new public DTOActEntityRole ToDTO() => new(this);

	public DTOActEntityRole ToDTO(string? applicationName) => new(this, applicationName);
}