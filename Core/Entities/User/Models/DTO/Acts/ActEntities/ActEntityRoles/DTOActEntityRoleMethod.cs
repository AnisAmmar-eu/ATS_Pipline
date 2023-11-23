using Core.Entities.User.Dictionaries;
using Core.Entities.User.Models.DB.Acts.ActEntities.ActEntityRoles;

namespace Core.Entities.User.Models.DTO.Acts.ActEntities.ActEntityRoles;

public partial class DTOActEntityRole
{
	public DTOActEntityRole()
	{
		Type = ApplicationTypeRID.User;
	}

	public DTOActEntityRole(ActEntityRole actEntityRole)
	{
		Type = actEntityRole.ApplicationType;
	}

	public DTOActEntityRole(ActEntityRole actEntityRole, string? applicationName)
	{
		Type = actEntityRole.ApplicationType;
		Name = applicationName;
	}
}