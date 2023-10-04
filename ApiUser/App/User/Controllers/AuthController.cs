using Core.Entities.User.Models.DTO.Auth.Login;
using Core.Entities.User.Models.DTO.Auth.Register;
using Core.Entities.User.Services.Auth;
using Core.Shared.Exceptions;
using Core.Shared.Models.HttpResponse;
using Microsoft.AspNetCore.Mvc;

namespace ApiUser.App.User.Controllers;

/// <summary>
///     Auth Routes
/// </summary>
[Route("apiUser/auth")]
[ApiController]
public class AuthController : ControllerBase
{
	private readonly IAuthService _authService;

	/// <summary>
	///     Auth Constructor
	/// </summary>
	/// <param name="authService"></param>
	public AuthController(IAuthService authService)
	{
		_authService = authService;
	}

	// POST apiUser/auth/register
	/// <summary>
	///     Register a new user
	/// </summary>
	/// <param name="dtoRegister"></param>
	/// <returns>A string</returns>
	[HttpPost("register")]
	public async Task<IActionResult> Register([FromBody] DTORegister dtoRegister)
	{
		try
		{
			await _authService.Register(dtoRegister);
		}
		catch (Exception e)
		{
			if (e is EntityNotFoundException)
				return new ApiResponseObject(e.Message).BadRequestResult();

			return new ApiResponseObject(500, e.Message).ErrorResult();
		}

		return new ApiResponseObject("User {" + dtoRegister.Username + "} has been successfully created.")
			.SuccessResult();
	}

	// POST apiUser/auth/login
	/// <summary>
	///     Login a user
	/// </summary>
	/// <param name="dtoLogin"></param>
	/// <returns>The token</returns>
	[HttpPost("login")]
	public async Task<IActionResult> Login([FromBody] DTOLogin dtoLogin)
	{
		DTOLoginResponse result;
		try
		{
			result = await _authService.Login(dtoLogin);
		}
		catch (UnauthorizedAccessException)
		{
			try
			{
				result = await _authService.RegisterSource(dtoLogin);
				return new ApiResponseObject(result).SuccessResult();
			}
			catch (EntityNotFoundException e2)
			{
				return new ApiResponseObject(e2.Message).BadRequestResult();
			}
			catch (Exception e2)
			{
				if (e2 is UnauthorizedAccessException) return Unauthorized(e2.Message);

				return new ApiResponseObject(500, "An undefined error happened.").ErrorResult();
			}
		}
		catch (Exception e)
		{
			return Unauthorized(e.Message);
		}

		return new ApiResponseObject(result).SuccessResult();
	}
}