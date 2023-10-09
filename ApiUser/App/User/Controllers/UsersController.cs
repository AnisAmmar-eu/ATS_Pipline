using System.ComponentModel.DataAnnotations;
using Core.Entities.User.Dictionaries;
using Core.Entities.User.Models.DTO.Users;
using Core.Entities.User.Services.Users;
using Core.Shared.Authorize;
using Core.Shared.Exceptions;
using Core.Shared.Models.HttpResponse;
using Microsoft.AspNetCore.Mvc;

namespace ApiUser.App.User.Controllers;

/// <summary>
///     Users Routes
/// </summary>
[Route("apiUser/users")]
[ApiController]
public class UsersController : ControllerBase
{
	private readonly IUsersService _usersService;

	/// <summary>
	///     Users Constructor
	/// </summary>
	public UsersController(IUsersService usersService)
	{
		_usersService = usersService;
	}

	#region Admin

	// PUT apiUser/users/{username}/admin
	/// <summary>
	///     Set or remove a user admin rights by username
	/// </summary>
	/// <param name="username"></param>
	/// <param name="toAdmin"></param>
	/// <returns>True/False</returns>
	[HttpPut("{username}/admin")]
	[ActAuthorize(ActionRID.ADMIN_GENERAL_RIGHTS)]
	public async Task<IActionResult> SetAdmin(string username, [FromBody] bool toAdmin)
	{
		bool result;
		try
		{
			result = await _usersService.SetAdmin(username, toAdmin);
		}
		catch (Exception e)
		{
			if (e is EntityNotFoundException)
				return new ApiResponseObject(e.Message).BadRequestResult();

			return new ApiResponseObject(500, e.Message).ErrorResult();
		}

		return new ApiResponseObject(result).SuccessResult();
	}

	#endregion

	#region AD

	// GET apiUser/users/ad
	/// <summary>
	///     Get all users from the Active Directory
	/// </summary>
	/// <returns></returns>
	[HttpGet("ad")]
	public IActionResult GetAllFromAD()
	{
		List<DTOUser> dtoUsers;
		try
		{
			dtoUsers = _usersService.GetAllFromAD();
		}
		catch (Exception e)
		{
			if (e is EntityNotFoundException)
				return new ApiResponseObject(e.Message).BadRequestResult();

			return new ApiResponseObject(500, e.Message).ErrorResult();
		}

		return new ApiResponseObject(dtoUsers).SuccessResult();
	}

	#endregion

	#region CRUD

	// GET apiUser/users
	/// <summary>
	///     Get all users
	/// </summary>
	/// <returns>A <see cref="List{DTOUser}" /></returns>
	[HttpGet]
	public async Task<IActionResult> GetAll()
	{
		List<DTOUser> dtoUsers;
		try
		{
			dtoUsers = await _usersService.GetAll();
		}
		catch (Exception e)
		{
			if (e is EntityNotFoundException)
				return new ApiResponseObject(e.Message).BadRequestResult();

			return new ApiResponseObject(500, e.Message).ErrorResult();
		}

		return new ApiResponseObject(dtoUsers).SuccessResult();
	}

	// GET apiUser/users/{username}
	/// <summary>
	///     Get a specific user by its username
	/// </summary>
	/// <param name="username"></param>
	/// <returns>A <see cref="DTOUser" /></returns>
	[HttpGet("{name}")]
	public async Task<IActionResult> GetByUsername([Required] string username)
	{
		DTOUser dtoUser;

		try
		{
			dtoUser = await _usersService.GetByUsername(username);
		}
		catch (Exception e)
		{
			if (e is EntityNotFoundException)
				return new ApiResponseObject(e.Message).BadRequestResult();

			return new ApiResponseObject(500, e.Message).ErrorResult();
		}

		return new ApiResponseObject(dtoUser).SuccessResult();
	}

	// GET apiUser/users/{id}
	/// <summary>
	///     Get a specific user by ID
	/// </summary>
	/// <param name="id"></param>
	/// <returns>A <see cref="DTOUser" /></returns>
	[HttpGet("{id}")]
	public async Task<IActionResult> GetById([Required] string id)
	{
		DTOUser dtoUser;

		try
		{
			dtoUser = await _usersService.GetById(id);
		}
		catch (Exception e)
		{
			if (e is EntityNotFoundException)
				return new ApiResponseObject(e.Message).BadRequestResult();

			return new ApiResponseObject(500, e.Message).ErrorResult();
		}

		return new ApiResponseObject(dtoUser).SuccessResult();
	}

	// PUT apiUser/users/{username}
	/// <summary>
	///     Update an existing user by its username
	/// </summary>
	/// <param name="username"></param>
	/// <param name="dtoUser"></param>
	/// <returns>The updated <see cref="DTOUser" /></returns>
	[HttpPut("{username}")]
	public async Task<IActionResult> Update([Required] string username, [FromBody] DTOUser dtoUser)
	{
		try
		{
			dtoUser = await _usersService.Update(username, dtoUser);
		}
		catch (Exception e)
		{
			if (e is EntityNotFoundException)
				return new ApiResponseObject(e.Message).BadRequestResult();

			return new ApiResponseObject(500, e.Message).ErrorResult();
		}

		return new ApiResponseObject(dtoUser).SuccessResult();
	}

	// DELETE apiUser/users/{username}
	/// <summary>
	///     Delete an existing user by its username
	/// </summary>
	/// <param name="username"></param>
	/// <returns>A <see cref="string" /></returns>
	[HttpDelete("{username}")]
	public async Task<IActionResult> Delete(string username)
	{
		try
		{
			await _usersService.Delete(username);
		}
		catch (Exception e)
		{
			if (e is EntityNotFoundException)
				return new ApiResponseObject(e.Message).BadRequestResult();

			return new ApiResponseObject(500, e.Message).ErrorResult();
		}

		return new ApiResponseObject("User [" + username + "] has been deleted.").SuccessResult();
	}

	#endregion

	#region Password

	// PUT apiUser/users/{username}/password
	/// <summary>
	///     Update a password by an admin with username
	/// </summary>
	/// <param name="username"></param>
	/// <param name="newPassword"></param>
	/// <returns>A <see cref="string" /></returns>
	[HttpPut("{username}/password")]
	public async Task<IActionResult> UpdatePasswordByAdmin(string username, [FromBody] string newPassword)
	{
		try
		{
			await _usersService.UpdatePasswordByAdmin(username, newPassword);
		}
		catch (Exception e)
		{
			if (e is EntityNotFoundException)
				return new ApiResponseObject(e.Message).BadRequestResult();

			return new ApiResponseObject(500, e.Message).ErrorResult();
		}

		return new ApiResponseObject("The password has been updated.").SuccessResult();
	}

	// PUT apiUser/users/{username}/password/user
	/// <summary>
	///     Update a password by a user with username
	/// </summary>
	/// <param name="username"></param>
	/// <param name="data"></param>
	/// <returns>A <see cref="string" /></returns>
	[HttpPut("{username}/password/user")]
	public async Task<IActionResult> UpdatePasswordByUser(string username, [FromBody] Dictionary<string, string> data)
	{
		try
		{
			string oldPassword = data["oldPassword"];
			string newPassword = data["newPassword"];
			await _usersService.UpdatePasswordByUser(username, oldPassword, newPassword);
		}
		catch (Exception e)
		{
			if (e is EntityNotFoundException)
				return new ApiResponseObject(e.Message).BadRequestResult();

			return new ApiResponseObject(500, e.Message).ErrorResult();
		}

		return new ApiResponseObject("The password has been updated.").SuccessResult();
	}

	// POST apiUser/users/{username}/password/reset
	/// <summary>
	///     Reset a password of a user
	/// </summary>
	/// <param name="username"></param>
	/// <returns></returns>
	[HttpPost("{username}/password/reset")]
	public async Task<IActionResult> ResetPassword(string username)
	{
		try
		{
			await _usersService.ResetPassword(username);
		}
		catch (Exception e)
		{
			if (e is EntityNotFoundException)
				return new ApiResponseObject(e.Message).BadRequestResult();

			return new ApiResponseObject(500, e.Message).ErrorResult();
		}

		return new ApiResponseObject("The password has been reset.").SuccessResult();
	}

	#endregion
}