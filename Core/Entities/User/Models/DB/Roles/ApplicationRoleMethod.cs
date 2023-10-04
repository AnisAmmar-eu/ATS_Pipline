using Core.Entities.User.Dictionary;
using Core.Entities.User.Models.DTO.Roles;
using Microsoft.AspNetCore.Identity;

namespace Core.Entities.User.Models.DB.Roles;

public partial class ApplicationRole : IdentityRole
{
	public ApplicationRole()
	{
	}

	public ApplicationRole(string rid)
	{
		Name = rid;
		Type = ApplicationRoleType.USER;
	}

	public DTORole ToDTO()
	{
		return new DTORole(this);
	}
}