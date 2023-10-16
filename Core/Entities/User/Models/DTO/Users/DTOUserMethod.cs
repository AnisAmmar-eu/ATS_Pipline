using Core.Entities.User.Models.DB.Users;

namespace Core.Entities.User.Models.DTO.Users;

public partial class DTOUser
{
	public DTOUser()
	{
		Username = "";
	}

	public DTOUser(ApplicationUser user)
	{
		Username = user.UserName;
		Firstname = user.Firstname;
		Lastname = user.Lastname;
	}
}