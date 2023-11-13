using System.ComponentModel.DataAnnotations;
using Core.Entities.IOT.IOTTags.Models.DTO;
using Core.Entities.IOT.IOTTags.Models.Structs;
using Core.Entities.IOT.IOTTags.Services;
using Core.Shared.Models.HttpResponse;
using Core.Shared.Services.System.Logs;
using Microsoft.AspNetCore.Mvc;

namespace ApiIOT.Controllers;

[ApiController]
[Route("apiIOT/iotTags")]
public class IOTTagController : ControllerBase
{
	private readonly IIOTTagService _iotTagService;
	private readonly ILogService _logService;

	public IOTTagController(IIOTTagService iotTagService, ILogService logService)
	{
		_iotTagService = iotTagService;
		_logService = logService;
	}

	[HttpGet("rid/{rid}")]
	public async Task<IActionResult> GetTagValueByRID([FromRoute] [Required] string rid)
	{
		DTOIOTTag tag;
		try
		{
			tag = await _iotTagService.GetByRID(rid);
		}
		catch (Exception e)
		{
			return await new ApiResponseObject().ErrorResult(_logService, ControllerContext, e);
		}

		return await new ApiResponseObject(tag).SuccessResult(_logService, ControllerContext);
	}

	[HttpPut("rids")]
	public async Task<IActionResult> GetTagValueByArrayRID([FromBody] [Required] IEnumerable<string> rids)
	{
		List<DTOIOTTag> tags;
		try
		{
			tags = await _iotTagService.GetByArrayRID(rids);
		}
		catch (Exception e)
		{
			return await new ApiResponseObject().ErrorResult(_logService, ControllerContext, e);
		}

		return await new ApiResponseObject(tags).SuccessResult(_logService, ControllerContext);
	}

	[HttpGet("id/{id}")]
	public async Task<IActionResult> GetTagValueByID([FromRoute] [Required] int id)
	{
		DTOIOTTag tag;
		try
		{
			tag = await _iotTagService.GetByID(id);
		}
		catch (Exception e)
		{
			return await new ApiResponseObject().ErrorResult(_logService, ControllerContext, e);
		}

		return await new ApiResponseObject(tag).SuccessResult(_logService, ControllerContext);
	}

	[HttpPut("{rid}/{value}")]
	public async Task<IActionResult> SetTagValueByRID([FromRoute] string rid, [FromRoute] string value)
	{
		DTOIOTTag dtoTag;
		try
		{
			dtoTag = await _iotTagService.UpdateTagByRID(rid, value);
		}
		catch (Exception e)
		{
			return await new ApiResponseObject().ErrorResult(_logService, ControllerContext, e);
		}

		return await new ApiResponseObject(dtoTag).SuccessResult(_logService, ControllerContext);
	}

	[HttpPut]
	public async Task<IActionResult> SetTagsValues([FromBody] [Required] List<PatchIOTTag> toUpdate)
	{
		List<DTOIOTTag> dtoTags;
		try
		{
			dtoTags = await _iotTagService.UpdateTags(toUpdate);
		}
		catch (Exception e)
		{
			return await new ApiResponseObject().ErrorResult(_logService, ControllerContext, e);
		}

		return await new ApiResponseObject(dtoTags).SuccessResult(_logService, ControllerContext);
	}
}