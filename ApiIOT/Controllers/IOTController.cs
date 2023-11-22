using Carter;
using Core.Shared.Models.HttpResponse;
using Microsoft.AspNetCore.Mvc;

namespace ApiIOT.Controllers;

public class IOTController : ICarterModule
{
	public void AddRoutes(IEndpointRouteBuilder app)
	{
		app.MapGroup("apiIOT").WithTags(nameof(IOTController)).MapGet("status", GetStatus);
	}

	private static IActionResult GetStatus()
	{
		return new ControllerResponseObject().SuccessResult();
	}
}