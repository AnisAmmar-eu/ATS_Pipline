using Core.Entities.User.Models.DB.Roles;

namespace Core.Entities.User.Models.DTO.Roles;

public partial class DTORole
{
	public DTORole()
	{
		Name = "";
		RID = "";
	}

	public DTORole(ApplicationRole role)
	{
		RID = role.Name;
		// TODO Name & Desc
	}
}