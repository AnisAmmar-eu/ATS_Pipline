using System.DirectoryServices.AccountManagement;
using System.Runtime.Versioning;
using Core.Entities.User.Dictionaries;
using Core.Entities.User.Models.DTO.Auth.Register;
using Core.Entities.User.Models.DTO.Users;

namespace Core.Entities.User.Models.DB.Users;

public partial class ApplicationUser
{
	public ApplicationUser()
	{
		Source = SourceAuth.EKIDI;
	}

	public ApplicationUser(DTORegister dto)
	{
		UserName = dto.Username;
		Firstname = dto.Firstname;
		Lastname = dto.Lastname;
		IsEkium = true;
		Source = SourceAuth.EKIDI;
	}

	[SupportedOSPlatform("windows")]
	public ApplicationUser(UserPrincipal userData)
	{
		UserName = userData.SamAccountName;
		Lastname = userData.Surname;
		Firstname = userData.GivenName;
		Source = SourceAuth.AD;
	}

	public DTOUser ToDTO()
	{
		return new DTOUser(this);
	}
}