using Core.Entities.User.Models.DB.Users;

namespace Core.Entities.User.Models.DTO.Users;

public partial class DTOUser
{
	public DTOUser()
	{
	}

	public DTOUser(ApplicationUser user)
	{
		Username = user.UserName ?? string.Empty;
		Firstname = user.Firstname;
		Lastname = user.Lastname;
	}
}