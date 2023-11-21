using Core.Shared.Models.DTO.System.Logs;
using Core.Shared.Models.HttpResponse;
using Core.Shared.Services.System.Logs;
using Microsoft.AspNetCore.Mvc;

namespace ApiStationCycle.Controllers;

[ApiController]
[Route("apiStationCycle/logs")]
public class LogsController : ControllerBase
{
	private readonly ILogService _logService;

	public LogsController(ILogService logService)
	{
		_logService = logService;
	}

	[HttpGet]
	public async Task<IActionResult> GetAll()
	{
		List<DTOLog> logs;
		try
		{
			logs = await _logService.GetAll();
		}
		catch (Exception e)
		{
			return await new ControllerResponseObject().ErrorResult(_logService, ControllerContext, e);
		}

		// No logs for this.
		return new ControllerResponseObject(logs).SuccessResult();
	}

	[HttpGet("{startPosition}/{nbOfItems}")]
	public async Task<IActionResult> GetRange([FromRoute] int startPosition, [FromRoute] int nbOfItems)
	{
		List<DTOLog> logs;
		try
		{
			logs = await _logService.GetRange(startPosition, nbOfItems);
		}
		catch (Exception e)
		{
			return await new ControllerResponseObject().ErrorResult(_logService, ControllerContext, e);
		}

		// No logs for this.
		return new ControllerResponseObject(logs).SuccessResult();
	}

	[HttpDelete]
	public async Task<IActionResult> DeleteAll()
	{
		try
		{
			await _logService.DeleteAllLogs();
		}
		catch (Exception e)
		{
			return await new ControllerResponseObject().ErrorResult(_logService, ControllerContext, e);
		}

		return await new ControllerResponseObject().SuccessResult(_logService, ControllerContext);
	}
}