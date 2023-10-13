using System.ComponentModel.DataAnnotations;
using Core.Entities.IOT.IOTTags.Models.DTO;
using Core.Entities.IOT.IOTTags.Models.Structs;
using Core.Entities.IOT.IOTTags.Services;
using Core.Shared.Models.HttpResponse;
using Core.Shared.Services.System.Logs;
using Microsoft.AspNetCore.Mvc;

namespace ApiAlarms.Controllers;

[ApiController]
[Route("api/iot-tag")]
public class IOTTagController : ControllerBase
{
	private IIOTTagService _iotTagService;
	private ILogsService _logsService;

	public IOTTagController(IIOTTagService iotTagService, ILogsService logsService)
	{
		_iotTagService = iotTagService;
		_logsService = logsService;
	}

	[HttpPatch]
	public async Task<IActionResult> SetTagsValues([FromBody] [Required] List<PatchIOTTag> toUpdate)
	{
		List<DTOIOTTag> dtoTags;
		try
		{
			dtoTags = await _iotTagService.UpdateTags(toUpdate);
		}
		catch (Exception e)
		{
			return await new ApiResponseObject().ErrorResult(_logsService, ControllerContext, e);
		}

		return await new ApiResponseObject(dtoTags).SuccessResult(_logsService, ControllerContext);
	}
}