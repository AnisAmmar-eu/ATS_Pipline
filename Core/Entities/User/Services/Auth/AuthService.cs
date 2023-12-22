using System.DirectoryServices.AccountManagement;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime.Versioning;
using System.Security.Claims;
using Core.Entities.User.Dictionaries;
using Core.Entities.User.Models.DB.Roles;
using Core.Entities.User.Models.DB.Users;
using Core.Entities.User.Models.DTO.Auth.Login;
using Core.Entities.User.Models.DTO.Auth.Register;
using Core.Shared.Exceptions;
using Core.Shared.Services.Jwt;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace Core.Entities.User.Services.Auth;

public class AuthService : IAuthService
{
	private readonly IConfiguration _configuration;
	private readonly IJwtService _jwtService;
	private readonly RoleManager<ApplicationRole> _roleManager;
	private readonly UserManager<ApplicationUser> _userManager;

	public AuthService(
		UserManager<ApplicationUser> userManager,
		RoleManager<ApplicationRole> roleManager,
		IConfiguration configuration,
		IJwtService jwtService)
	{
		_userManager = userManager;
		_roleManager = roleManager;
		_configuration = configuration;
		_jwtService = jwtService;
	}

    /// <summary>
    ///     Create a User and add Roles to him
    /// </summary>
    /// <param name="dtoRegister"></param>
    /// <exception cref="EntityNotFoundException"></exception>
    public async Task Register(DTORegister dtoRegister)
    {
        if (dtoRegister.Username is null)
            throw new EntityNotFoundException("Empty values.");

        ApplicationUser? user = await _userManager.FindByNameAsync(dtoRegister.Username);
        if (user is not null)
            throw new EntityNotFoundException("Username already used.");

        if (dtoRegister.Password is null)
            throw new NoDataException("No password has been given.");

        // Create User
        user = dtoRegister.ToUser();
        IdentityResult identityResult = await _userManager.CreateAsync(user, dtoRegister.Password);
        if (!identityResult.Succeeded)
            throw new EntityNotFoundException("Error during user creation.");

        // Add Roles
        if (dtoRegister.Roles.Count == 0)
            return;

        IdentityResult roleTask = await _userManager.AddToRolesAsync(user, dtoRegister.Roles);

        if (!roleTask.Succeeded)
            throw new EntityNotFoundException("Error during roles association.");
    }

    /// <summary>
    ///     Check credentials (username + password)
    /// </summary>
    /// <param name="dtoLogin"></param>
    /// <returns>The user</returns>
    /// <exception cref="UnauthorizedAccessException"></exception>
    public async Task<ApplicationUser> CheckCredentials(DTOLogin dtoLogin)
	{
		ApplicationUser? user = await _userManager.FindByNameAsync(dtoLogin.Username);
		if (user is null)
			throw new UnauthorizedAccessException("Invalid credentials");

		bool isValid = await _userManager.CheckPasswordAsync(user, dtoLogin.Password);
		if (!isValid)
			throw new UnauthorizedAccessException("Invalid credentials");

		return user;
	}

	[SupportedOSPlatform("windows")]
	public UserPrincipal? CheckCredentialsAD(DTOLogin dtoLogin)
	{
		PrincipalContext adContext = new(ContextType.Domain, _configuration["AdHost"]);

		bool res = adContext.ValidateCredentials(dtoLogin.Username, dtoLogin.Password);
		return (!res) ? null : UserPrincipal.FindByIdentity(adContext, dtoLogin.Username);
	}

	/// <summary>
	///     Register a user with one of our sources
	/// </summary>
	/// <param name="model"></param>
	/// <returns>A <see cref="DTOLoginResponse" /> with the token</returns>
	/// <exception cref="UnauthorizedAccessException"></exception>
	[SupportedOSPlatform("windows")]
	public async Task<DTOLoginResponse> RegisterSource(DTOLogin model)
	{
		if (model is null)
            throw new UnauthorizedAccessException("Invalid credentials");

        // Try register with all sources except EKIDI
        foreach (string source in SourceAuth.GetSources())
		{
			if (source == SourceAuth.Ekidi)
				continue;

			ApplicationUser? user = source switch {
				SourceAuth.AD => await RegisterAD(model),
				_ => null,
			};

			if (user is null)
				continue;

			return ClaimsToken(user);
		}

		throw new UnauthorizedAccessException("Invalid credentials");
	}

	/// <summary>
	///     Verify password + generate a token valid for 3 hours
	/// </summary>
	/// <param name="model"></param>
	/// <exception cref="UnauthorizedAccessException"></exception>
	/// <exception cref="EntityNotFoundException"></exception>
	[SupportedOSPlatform("windows")]
	public async Task<DTOLoginResponse> Login(DTOLogin model)
	{
		ApplicationUser? user = await _userManager.FindByNameAsync(model.Username);
		if (user is null)
			throw new UnauthorizedAccessException("Invalid credentials");

		dynamic? confirmUser = user.Source switch {
			SourceAuth.Ekidi => await CheckCredentials(model),
			SourceAuth.AD => CheckCredentialsAD(model),
			_ => throw new EntityNotFoundException("Source not found."),
		};

		if (confirmUser is null)
			throw new EntityNotFoundException("Invalid credentials");

		return ClaimsToken(user);
	}

	/// <summary>
	///     Get all roles of a user
	/// </summary>
	/// <param name="user"></param>
	/// <returns>A list of roles</returns>
	public Task<IList<string>> GetRoles(ApplicationUser user)
	{
		return _userManager.GetRolesAsync(user);
	}

	/// <summary>
	///     Get any property from the httpContext claims
	/// </summary>
	/// <param name="httpContext"></param>
	/// <param name="property"></param>
	/// <returns>The value of the property</returns>
	public string? GetUserPropertyFromContext(HttpContext httpContext, string property)
	{
		return httpContext.User.Claims.Where(x => x.Type == property).Select(c => c.Value).FirstOrDefault();
	}

	/// <summary>
	///     Set httpContext items with user values
	/// </summary>
	/// <param name="httpContext"></param>
	/// <exception cref="UnauthorizedAccessException"></exception>
	public async Task SetContextWithUser(HttpContext httpContext)
	{
		string? userId = httpContext.User.Claims.Where(x => x.Type == "Id").Select(c => c.Value).FirstOrDefault();
		if (userId is null)
			throw new UnauthorizedAccessException("Invalid user.");

		ApplicationUser? user = await _userManager.FindByIdAsync(userId);

		if (user is null)
			throw new UnauthorizedAccessException("Invalid user.");

		httpContext.Items["UserId"] = userId;

		// Get RoleId from RoleName in token
		// [TO CHECK] -> What is the best ? Get user roles here on each request or during login
		// + -> By getting user roles here, we can update his roles without logout
		// OLD -> httpContext.User.Claims.Where(x => x.Type == ClaimTypes.Role).Select(c => c.Value).ToList()

		httpContext.Items["UserRolesId"] = await GetRolesIdFromUser(user, httpContext);
	}

	/// <summary>
	///     Get all roles of a connected user + check if it is an admin
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
			ApplicationRole? role = await _roleManager.FindByNameAsync(roleName);
			switch (role)
			{
				case null:
					continue;
				case { Type: ApplicationRoleType.SystemFives or ApplicationRoleType.SystemATS }:
					httpContext.Items["HasAdminRole"] = true;
					break;
			}

			userRolesID.Add(role.Id);
		}

		return userRolesID;
	}

    /// <summary>
    ///     Verify password + generate a token valid for 3 hours
    /// </summary>
    /// <param name="user"></param>
    /// <returns>A <see cref="DTOLoginResponse" /> with the token</returns>
    private DTOLoginResponse ClaimsToken(ApplicationUser user)
	{
		// Set claims list to add in the token
		List<Claim> authClaims = [
			new("Id", user.Id),
			new(ClaimTypes.Name, user.UserName ?? string.Empty),
			new("Username", user.UserName ?? string.Empty),
			new("Firstname", user.Firstname ?? string.Empty),
			new("Lastname", user.Lastname ?? string.Empty),
			new("IsEkium", user.IsEkium.ToString()),
			new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
		];

		// Check if in dev mode
		int time = 180;
		if (_configuration["ASPNETCORE_ENVIRONMENT"] == "Development")
            time = 999999;

        string token = _jwtService.GenerateToken(authClaims, time);

		return new(token);
	}

	[SupportedOSPlatform("windows")]
	private async Task<ApplicationUser?> RegisterAD(DTOLogin model)
	{
		UserPrincipal? userData = CheckCredentialsAD(model);

		if (userData is null)
			return null;

		ApplicationUser user = new(userData);

		IdentityResult identityResult = await _userManager.CreateAsync(user);
		return (!identityResult.Succeeded) ? null : user;
	}
}