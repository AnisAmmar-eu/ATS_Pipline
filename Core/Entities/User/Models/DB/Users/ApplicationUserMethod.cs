using System.DirectoryServices.AccountManagement;
using Core.Entities.User.Dictionaries;
using Core.Entities.User.Models.DTO.Auth.Register;
using Core.Entities.User.Models.DTO.Users;
using Microsoft.AspNetCore.Identity;

namespace Core.Entities.User.Models.DB.Users;

public partial class ApplicationUser : IdentityUser
{
	public ApplicationUser()
	{
		Source = SourceAuth.EKIDI;
	}

	public ApplicationUser(DTORegister dto)
	{
		UserName = dto.Username;
		Email = dto.Email;
		Firstname = dto.Firstname;
		Lastname = dto.Lastname;
		IsEkium = true;
		Source = SourceAuth.EKIDI;
	}

	public ApplicationUser(UserPrincipal userData)
	{
		UserName = userData.SamAccountName;
		Lastname = userData.Surname;
		Firstname = userData.GivenName;
		Email = userData.EmailAddress;
		Source = SourceAuth.AD;
	}

	public DTOUser ToDTO()
	{
		return new DTOUser(this);
	}
}