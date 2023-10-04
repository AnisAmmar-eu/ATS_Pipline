using Core.Entities.User.Models.DB.Users;

namespace Core.Entities.User.Models.DTO.Auth.Register;

public partial class DTORegister
{
	public ApplicationUser ToUser()
	{
		return new ApplicationUser(this);
	}
}