using Core.Shared.Models.HttpResponse;
using Microsoft.AspNetCore.Mvc;

namespace ApiUser.App;

/// <summary>
/// Api for general API operations eg. get status.
/// </summary>
[ApiController]
[Route("apiUser")]
public class ApiUserController : ControllerBase
{
	/// <summary>
	/// Returns 200. Useful to know if the API is down or not.
	/// </summary>
	/// <returns></returns>
	[HttpGet("status")]
	public IActionResult GetStatus()
	{
		return new ApiResponseObject().SuccessResult();
	}
}