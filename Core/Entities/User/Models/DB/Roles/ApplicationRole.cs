using Microsoft.AspNetCore.Identity;

namespace Core.Entities.User.Models.DB.Roles;

public partial class ApplicationRole : IdentityRole
{
	public string Type { get; set; } = "USER";
}