using Core.Entities.User.Models.DTO.Roles;

namespace Core.Entities.User.Models.DTO.Users;

public partial class DTOUser
{
	public string Username { get; set; } = string.Empty;
	public string? Firstname { get; set; }
	public string? Lastname { get; set; }
	public List<DTORole> Roles { get; set; } = new();
}