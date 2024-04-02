using System.ComponentModel.DataAnnotations;
using Carter;
using Core.Entities.User.Dictionaries;
using Core.Entities.User.Models.DTO.Users;
using Core.Entities.User.Services.Users;
using Core.Shared.Endpoints.Kernel;
using Core.Shared.Models.ApiResponses;
using Core.Shared.Services.SystemApp.Logs;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ApiUser.Endpoints;

/// <summary>
///     Users Routes
/// </summary>
public class UserEndpoint : BaseEndpoint, ICarterModule
{
	/// <summary>
	///     Add Routes from CarterModule
	/// </summary>
	/// <param name="app"></param>
	/// <exception cref="NotImplementedException"></exception>
	public void AddRoutes(IEndpointRouteBuilder app)
	{
		RouteGroupBuilder group = app.MapGroup("apiUser/users").WithTags(nameof(UserEndpoint));

		group.MapPut("{username}/admin", SetAdmin).RequireAuthorization(ActionRID.AdminGeneralRights);
		group.MapGet("ad", GetAllFromAD);
		group.MapGet(string.Empty, GetAll);
		group.MapGet("username/{username}", GetByUsername);
		group.MapGet("id/{id}", GetById);
		group.MapPut("{username}", Update);
		group.MapDelete("{username}", Delete);
		group.MapPut("{username}/password", UpdatePasswordByAdmin);
		group.MapPut("{username}/password/user", UpdatePasswordByUser);
		group.MapPost("{username}/password/reset", ResetPassword);
	}

	#region Admin

	// PUT apiUser/users/{username}/admin
	/// <summary>
	///     Set or remove a user admin rights by username
	/// </summary>
	/// <param name="username"></param>
	/// <param name="toAdmin"></param>
	/// <param name="userService"></param>
	/// <param name="logService"></param>
	/// <param name="httpContext"></param>
	/// <returns>True/False</returns>
	private static Task<JsonHttpResult<ApiResponse>> SetAdmin(
		string username,
		[FromBody] bool toAdmin,
		IUserService userService,
		ILogService logService,
		HttpContext httpContext)
	{
		return GenericEndpoint(
			() => userService.SetAdmin(username, toAdmin),
			logService,
			httpContext);
	}

	#endregion

	#region AD

	// GET apiUser/users/ad
	/// <summary>
	///     Get all users from the Active Directory
	/// </summary>
	/// <param name="userService"></param>
	/// <param name="logService"></param>
	/// <param name="httpContext"></param>
	private static Task<JsonHttpResult<ApiResponse>> GetAllFromAD(
		IUserService userService,
		ILogService logService,
		HttpContext httpContext)
	{
		return GenericEndpoint(() => Task.FromResult(userService.GetAllFromAD()), logService, httpContext);
	}

	#endregion

	#region CRUD

	// GET apiUser/users
	/// <summary>
	///     Get all users
	/// </summary>
	/// <param name="userService"></param>
	/// <param name="logService"></param>
	/// <param name="httpContext"></param>
	/// <returns>A <see cref="List{DTOUser}" /></returns>
	private static Task<JsonHttpResult<ApiResponse>> GetAll(
		IUserService userService,
		ILogService logService,
		HttpContext httpContext)
	{
		return GenericEndpoint(userService.GetAll, logService, httpContext);
	}

	// GET apiUser/users/{username}
	/// <summary>
	///     Get a specific user by its username
	/// </summary>
	/// <param name="username"></param>
	/// <param name="userService"></param>
	/// <param name="logService"></param>
	/// <param name="httpContext"></param>
	/// <returns>A <see cref="DTOUser" /></returns>
	private static Task<JsonHttpResult<ApiResponse>> GetByUsername(
		[Required] string username,
		IUserService userService,
		ILogService logService,
		HttpContext httpContext)
	{
		return GenericEndpoint(() => userService.GetByUsername(username), logService, httpContext);
	}

	// GET apiUser/users/{id}
	/// <summary>
	///     Get a specific user by ID
	/// </summary>
	/// <param name="id"></param>
	/// <param name="userService"></param>
	/// <param name="logService"></param>
	/// <param name="httpContext"></param>
	/// <returns>A <see cref="DTOUser" /></returns>
	private static Task<JsonHttpResult<ApiResponse>> GetById(
		[Required] string id,
		IUserService userService,
		ILogService logService,
		HttpContext httpContext)
	{
		return GenericEndpoint(() => userService.GetById(id), logService, httpContext);
	}

	// PUT apiUser/users/{username}
	/// <summary>
	///     Update an existing user by its username
	/// </summary>
	/// <param name="username"></param>
	/// <param name="dtoUser"></param>
	/// <param name="userService"></param>
	/// <param name="logService"></param>
	/// <param name="httpContext"></param>
	/// <returns>The updated <see cref="DTOUser" /></returns>
	private static Task<JsonHttpResult<ApiResponse>> Update(
		[Required] string username,
		[FromBody] DTOUser dtoUser,
		IUserService userService,
		ILogService logService,
		HttpContext httpContext)
	{
		return GenericEndpoint(
			() => userService.Update(username, dtoUser),
			logService,
			httpContext);
	}

	// DELETE apiUser/users/{username}
	/// <summary>
	///     Delete an existing user by its username
	/// </summary>
	/// <param name="username"></param>
	/// <param name="userService"></param>
	/// <param name="logService"></param>
	/// <param name="httpContext"></param>
	/// <returns>A <see cref="string" /></returns>
	private static Task<JsonHttpResult<ApiResponse>> Delete(
		string username,
		IUserService userService,
		ILogService logService,
		HttpContext httpContext)
	{
		return GenericEndpoint(
			async () =>
			{
				await userService.Delete(username);
				return $"User {{{username}}} has been deleted.";
			},
			logService,
			httpContext);
	}

	#endregion

	#region Password

	// PUT apiUser/users/{username}/password
	/// <summary>
	///     Update a password by an admin with username
	/// </summary>
	/// <param name="username"></param>
	/// <param name="newPassword"></param>
	/// <param name="userService"></param>
	/// <param name="logService"></param>
	/// <param name="httpContext"></param>
	/// <returns>A <see cref="string" /></returns>
	private static Task<JsonHttpResult<ApiResponse>> UpdatePasswordByAdmin(
		string username,
		[FromBody] string newPassword,
		IUserService userService,
		ILogService logService,
		HttpContext httpContext)
	{
		return GenericEndpoint(
			async () =>
			{
				await userService.UpdatePasswordByAdmin(username, newPassword);
				return "The password has been updated.";
			},
			logService,
			httpContext);
	}

	// PUT apiUser/users/{username}/password/user
	/// <summary>
	///     Update a password by a user with username
	/// </summary>
	/// <param name="username"></param>
	/// <param name="data"></param>
	/// <param name="userService"></param>
	/// <param name="logService"></param>
	/// <param name="httpContext"></param>
	/// <returns>A <see cref="string" /></returns>
	private static Task<JsonHttpResult<ApiResponse>> UpdatePasswordByUser(
		string username,
		[FromBody] Dictionary<string, string> data,
		IUserService userService,
		ILogService logService,
		HttpContext httpContext)
	{
		return GenericEndpoint(
			async () =>
			{
				string oldPassword = data["oldPassword"];
				string newPassword = data["newPassword"];
				await userService.UpdatePasswordByUser(username, oldPassword, newPassword);
				return "The password has been updated.";
			},
			logService,
			httpContext);
	}

	// POST apiUser/users/{username}/password/reset
	/// <summary>
	///     Reset a password of a user
	/// </summary>
	/// <param name="username"></param>
	/// <param name="userService"></param>
	/// <param name="logService"></param>
	/// <param name="httpContext"></param>
	private static Task<JsonHttpResult<ApiResponse>> ResetPassword(
		string username,
		IUserService userService,
		ILogService logService,
		HttpContext httpContext)
	{
		return GenericEndpoint(
			async () =>
			{
				await userService.ResetPassword(username);
				return "The password has been reset.";
			},
			logService,
			httpContext);
	}

	#endregion
}