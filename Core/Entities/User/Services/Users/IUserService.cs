using Core.Entities.User.Models.DB.Users;
using Core.Entities.User.Models.DTO.Roles;
using Core.Entities.User.Models.DTO.Users;

namespace Core.Entities.User.Services.Users;

public interface IUserService
{
	Task<List<DTOUser>> GetAll();
	Task<DTOUser> GetByUsername(string username);
	Task<DTOUser> GetById(string id);
	Task<DTOUser> Update(string username, DTOUser dtoUser);
	Task Delete(string username);
	Task<string> GetIdByUsername(string username);
	Task<List<DTORole>> GetRolesByUsername(string username);
	Task<List<ApplicationUser>> GetAllByRole(string roleName);
	Task<bool> SetAdmin(string username, bool toAdmin);
	Task UpdatePasswordByAdmin(string username, string newPassword);
	Task UpdatePasswordByUser(string username, string oldPassword, string newPassword);
	Task ResetPassword(string name);
	List<DTOUser> GetAllFromAD();
}