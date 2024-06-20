using Carter;
using Core.Entities.User.Models.DTO.Auth.Login;
using Core.Entities.User.Models.DTO.Auth.Register;
using Core.Entities.User.Services.Auth;
using Core.Shared.Endpoints.Kernel;
using Core.Shared.Exceptions;
using Core.Shared.Models.ApiResponses;
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
	/// <param name="httpContext"></param>
	/// <returns>A string</returns>
	private static Task<JsonHttpResult<ApiResponse>> Register(
		[FromBody] DTORegister dtoRegister,
		IAuthService authService,
		HttpContext httpContext)
	{
		return GenericEndpoint(
			async () => {
				await authService.Register(dtoRegister);
				return $"User {{{dtoRegister.Username}}} has been successfully created.";
			},
			httpContext);
	}

	// POST apiUser/auth/login
	/// <summary>
	///     Login a user
	/// </summary>
	/// <param name="dtoLogin"></param>
	/// <param name="authService"></param>
	/// <param name="httpContext"></param>
	/// <returns>The token</returns>
	private static async Task<JsonHttpResult<ApiResponse>> Login(
		[FromBody] DTOLogin dtoLogin, IAuthService authService, HttpContext httpContext)
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
				return new ApiResponse(result).SuccessResult(httpContext);
			}
			catch (EntityNotFoundException e2)
			{
				return new ApiResponse(e2.Message).ErrorResult(httpContext, e2);
			}
			catch (Exception e2)
			{
				return (e2 is UnauthorizedAccessException)
					? new ApiResponse(e2.Message).ErrorResult(httpContext, e2)
					: new ApiResponse("An undefined error happened.").ErrorResult(httpContext, e2);
			}
		}
		catch (Exception e)
		{
			return new ApiResponse().ErrorResult(httpContext, e);
		}

		return new ApiResponse(result).SuccessResult(httpContext);
	}
}