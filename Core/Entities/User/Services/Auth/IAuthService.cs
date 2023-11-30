using System.DirectoryServices.AccountManagement;
using Core.Entities.User.Models.DB.Users;
using Core.Entities.User.Models.DTO.Auth.Login;
using Core.Entities.User.Models.DTO.Auth.Register;
using Microsoft.AspNetCore.Http;

namespace Core.Entities.User.Services.Auth;

public interface IAuthService
{
	Task Register(DTORegister dtoRegister);
	Task<ApplicationUser> CheckCredentials(DTOLogin dtoLogin);
	UserPrincipal? CheckCredentialsAD(DTOLogin dtoLogin);
	Task<DTOLoginResponse> RegisterSource(DTOLogin model);
	Task<DTOLoginResponse> Login(DTOLogin model);

	Task<IList<string>> GetRoles(ApplicationUser user);
	string? GetUserPropertyFromContext(HttpContext httpContext, string property);
	Task SetContextWithUser(HttpContext httpContext);
	Task<List<string>> GetRolesIdFromUser(ApplicationUser user, HttpContext httpContext);
}