using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Core.Entities.User.Models.DB;
using Core.Shared.Exceptions;
using Microsoft.AspNetCore.Http;
using Core.Entities.User.Dictionary;
using System.DirectoryServices.AccountManagement;
using Core.Entities.User.Models.DB.Roles;
using Core.Entities.User.Models.DB.Users;
using Core.Entities.User.Models.DTO.Auth.Login;
using Core.Entities.User.Models.DTO.Auth.Register;
using Core.Shared.Services.System.Jwt;

namespace Core.Entities.User.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly IJwtService _jwtService;

        public AuthService(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, IConfiguration configuration, IJwtService jwtService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _jwtService = jwtService;
        }

        /// <summary>
        /// Create a User and add Roles to him
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <exception cref="EntityNotFoundException"></exception>
        public async Task Register(DTORegister dtoRegister)
        {
            if (dtoRegister.Username == null || dtoRegister.Email == null)
                throw new EntityNotFoundException("Empty values.");

            var user = await _userManager.FindByEmailAsync(dtoRegister.Email);
            if (user != null)
                throw new EntityNotFoundException("Email already used.");

            user = await _userManager.FindByNameAsync(dtoRegister.Username);
            if (user != null)
                throw new EntityNotFoundException("Username already used.");

            // Create User
            user = dtoRegister.ToUser();

            var IdentityResult = await _userManager.CreateAsync(user, dtoRegister.Password);
            if (IdentityResult.Succeeded == false)
                throw new EntityNotFoundException("Error during user creation.");

            // Add Roles
            if (dtoRegister.Roles != null && dtoRegister.Roles.Count > 0)
            {
                var roleTask = await _userManager.AddToRolesAsync(user, dtoRegister.Roles);

                if (roleTask.Succeeded == false)
                    throw new EntityNotFoundException("Error during roles association.");
            }
        }

        /// <summary>
        /// Verify email and password + generate a token valid for 3 hours
        /// </summary>
        /// <param name="model"></param>
        /// <returns>A <see cref="DTOLoginResponse"/> with the token</returns>
        private DTOLoginResponse ClaimsToken(ApplicationUser user)
        {
            // Set claims list to add in the token
            var authClaims = new List<Claim>
            {
                new Claim("Id", user.Id),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim("Username", user.UserName),
                new Claim("Firstname", user.Firstname ?? ""),
                new Claim("Lastname", user.Lastname ?? ""),
                new Claim("IsEkium", user.IsEkium.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            // Check if in dev mode
            var time = 180;
            if (_configuration["ASPNETCORE_ENVIRONMENT"] == "Development")
            {
                time = 999999;
            }

            string token = _jwtService.GenerateToken(authClaims, time);

            return new DTOLoginResponse(token);
        }

        /// <summary>
        /// Check credentials (username + password)
        /// </summary>
        /// <param name="dtoLogin"></param>
        /// <returns>The user</returns>
        /// <exception cref="UnauthorizedAccessException"></exception>
        public async Task<ApplicationUser> CheckCredentials(DTOLogin dtoLogin)
        {
            ApplicationUser user = await _userManager.FindByNameAsync(dtoLogin.Username);
            if (user == null)
                throw new UnauthorizedAccessException("Invalid credentials");

            bool isValid = await _userManager.CheckPasswordAsync(user, dtoLogin.Password);
            if (!isValid)
                throw new UnauthorizedAccessException("Invalid credentials");

            return user;
        }

        public UserPrincipal? CheckCredentialsAD(DTOLogin dtoLogin)
        {
            PrincipalContext adContext = new PrincipalContext(ContextType.Domain, _configuration["AdHost"]);

            bool res = adContext.ValidateCredentials(dtoLogin.Username, dtoLogin.Password);
            if (!res)
                return null;

            UserPrincipal? userData = UserPrincipal.FindByIdentity(adContext, dtoLogin.Username);

            return userData;
        }

        private async Task<ApplicationUser?> RegisterAD(DTOLogin model)
        {
            UserPrincipal? userData = CheckCredentialsAD(model);

            if (userData == null)
                return null;

            var user = new ApplicationUser(userData);

            var IdentityResult = await _userManager.CreateAsync(user);
            if (IdentityResult.Succeeded == false)
                return null;

            return user;
        }

        /// <summary>
        /// Register a user with one of our sources
        /// </summary>
        /// <param name="model"></param>
        /// <returns>A <see cref="DTOLoginResponse"/> with the token</returns>
        /// <exception cref="UnauthorizedAccessException"></exception>
		public async Task<DTOLoginResponse> RegisterSource(DTOLogin model)
        {
            if (model == null) throw new UnauthorizedAccessException("Invalid credentials");

            // Try register with all sources except EKIDI
            foreach (string source in SourceAuth.GetSources())
            {
                if (source == SourceAuth.EKIDI)
                    continue;

                ApplicationUser? user = source switch
                {
                    SourceAuth.AD => await RegisterAD(model),
                    _ => null
                };

                if (user == null)
                    continue;

                return ClaimsToken(user);
            }

            throw new UnauthorizedAccessException("Invalid credentials");
        }

        /// <summary>
        /// Verify email and password + generate a token valid for 3 hours
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <exception cref="UnauthorizedAccessException"></exception>
        /// <exception cref="EntityNotFoundException"></exception>
        public async Task<DTOLoginResponse> Login(DTOLogin model)
        {
            ApplicationUser? user = await _userManager.FindByNameAsync(model.Username);
            if (user == null)
                throw new UnauthorizedAccessException("Invalid credentials");

            dynamic? confirmUser = (object)user.Source switch
            {
                SourceAuth.EKIDI => await CheckCredentials(model),
                SourceAuth.AD => CheckCredentialsAD(model),
                _ => throw new EntityNotFoundException("Source not found."),
            };

            if (confirmUser == null)
                throw new EntityNotFoundException("Invalid credentials");

            return ClaimsToken(user);
        }

        /// <summary>
        /// Get all roles of a user
        /// </summary>
        /// <param name="user"></param>
        /// <returns>A list of roles</returns>
        public async Task<IList<string>> GetRoles(ApplicationUser user)
        {
            return await _userManager.GetRolesAsync(user);
        }

        /// <summary>
        /// Get any property from the httpContext claims
        /// </summary>
        /// <param name="httpContext"></param>
        /// <param name="property"></param>
        /// <returns>The value of the property</returns>
		public string? GetUserPropertyFromContext(HttpContext httpContext, string property)
        {
            return httpContext.User.Claims.Where(x => x.Type == property).Select(c => c.Value).FirstOrDefault();
        }

        /// <summary>
        /// Set httpContext items with user values
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        /// <exception cref="UnauthorizedAccessException"></exception>
        public async Task SetContextWithUser(HttpContext httpContext)
        {
            string? userId = httpContext.User.Claims.Where(x => x.Type == "Id").Select(c => c.Value).FirstOrDefault();
            if (userId == null)
                throw new UnauthorizedAccessException("Invalid user.");

            ApplicationUser user = await _userManager.FindByIdAsync(userId);

            if (user == null)
                throw new UnauthorizedAccessException("Invalid user.");

            httpContext.Items["UserId"] = userId;

            // Get RoleId from RoleName in token
            // [TO CHECK] -> What is the best ? Get user roles here on each request or during login
            // + -> By getting user roles here, we can update his roles without logout
            // OLD -> httpContext.User.Claims.Where(x => x.Type == ClaimTypes.Role).Select(c => c.Value).ToList()

            httpContext.Items["UserRolesId"] = await GetRolesIdFromUser(user, httpContext);
        }

        /// <summary>
        /// Get all roles of a connected user + check if it is an admin
        /// </summary>
        /// <param name="user"></param>
        /// <param name="httpContext"></param>
        /// <returns>The list of roles</returns>
        public async Task<List<string>> GetRolesIdFromUser(ApplicationUser user, HttpContext httpContext)
        {
            List<string> userRolesID = new();

            httpContext.Items["HasAdminRole"] = false;

            foreach (string roleName in await GetRoles(user))
            {
                ApplicationRole role = await _roleManager.FindByNameAsync(roleName);
                if (role.Type == ApplicationRoleType.SYSTEM_EKIUM || role.Type == ApplicationRoleType.SYSTEM_EKIDI)
                {
                    httpContext.Items["HasAdminRole"] = true;
                }
                userRolesID.Add(role.Id);
            }
            return userRolesID;
        }
    }
}
