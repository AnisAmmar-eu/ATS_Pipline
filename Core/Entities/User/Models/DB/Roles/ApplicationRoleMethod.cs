using Core.Entities.User.Dictionaries;
using Core.Entities.User.Models.DTO.Roles;

namespace Core.Entities.User.Models.DB.Roles;

public partial class ApplicationRole
{
	public ApplicationRole()
	{
	}

	public ApplicationRole(string rid)
	{
		Name = rid;
		Type = ApplicationRoleType.User;
	}

	public DTORole ToDTO() => new(this);
}