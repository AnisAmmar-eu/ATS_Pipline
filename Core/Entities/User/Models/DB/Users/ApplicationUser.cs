using Microsoft.AspNetCore.Identity;

namespace Core.Entities.User.Models.DB.Users;

public partial class ApplicationUser : IdentityUser
{
	public string? Firstname { get; set; }
	public string? Lastname { get; set; }
	public bool IsEkium { get; set; }
	public string Source { get; set; }
}