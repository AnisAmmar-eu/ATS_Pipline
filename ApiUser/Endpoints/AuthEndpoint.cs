using Carter;
using Core.Entities.User.Models.DTO.Auth.Login;
using Core.Entities.User.Models.DTO.Auth.Register;
using Core.Entities.User.Services.Auth;
using Core.Shared.Endpoints.Kernel;
using Core.Shared.Exceptions;
using Core.Shared.Models.ApiResponses;
using Core.Shared.Services.System.Logs;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ApiUser.Endpoints;

/// <summary>
///     Auth Routes
/// </summary>
public class AuthEndpoint : BaseEndpoint, ICarterModule
{
	/// <summary>
	///     Add Routes from CarterModule
	/// </summary>
	/// <param name="app"></param>
	/// <exception cref="NotImplementedException"></exception>
	public void AddRoutes(IEndpointRouteBuilder app)
	{
		RouteGroupBuilder group = app.MapGroup("apiUser/auth").WithTags(nameof(AuthEndpoint));

		group.MapPost("register", Register);
		group.MapPost("login", Login);
	}

	// POST apiUser/auth/register
	/// <summary>
	///     Register a new user
	/// </summary>
	/// <param name="dtoRegister"></param>
	/// <param name="authService"></param>
	/// <param name="logService"></param>
	/// <param name="httpContext"></param>
	/// <returns>A string</returns>
	private static Task<JsonHttpResult<ApiResponse>> Register(
		[FromBody] DTORegister dtoRegister,
		IAuthService authService,
		ILogService logService,
		HttpContext httpContext)
	{
		return GenericEndpoint(
			async () =>
			{
				await authService.Register(dtoRegister);
				return $"User {{{dtoRegister.Username}}} has been successfully created.";
			},
			logService,
			httpContext);
	}

	// POST apiUser/auth/login
	/// <summary>
	///     Login a user
	/// </summary>
	/// <param name="dtoLogin"></param>
	/// <param name="authService"></param>
	/// <param name="logService"></param>
	/// <param name="httpContext"></param>
	/// <returns>The token</returns>
	[HttpPost("login")]
	private static async Task<JsonHttpResult<ApiResponse>> Login(
		[FromBody] DTOLogin dtoLogin, IAuthService authService, ILogService logService, HttpContext httpContext)
	{
		DTOLoginResponse result;
		try
		{
			result = await authService.Login(dtoLogin);
		}
		catch (UnauthorizedAccessException)
		{
			try
			{
				result = await authService.RegisterSource(dtoLogin);
				return await new ApiResponse(result).SuccessResult(logService, httpContext);
			}
			catch (EntityNotFoundException e2)
			{
				return await new ApiResponse(e2.Message).ErrorResult(logService, httpContext, e2);
			}
			catch (Exception e2)
			{
				if (e2 is UnauthorizedAccessException)
					return await new ApiResponse(e2.Message).ErrorResult(logService, httpContext, e2);

				return await new ApiResponse("An undefined error happened.").ErrorResult(logService, httpContext, e2);
			}
		}
		catch (Exception e)
		{
			return await new ApiResponse().ErrorResult(logService, httpContext, e);
		}

		return await new ApiResponse(result).SuccessResult(logService, httpContext);
	}
}