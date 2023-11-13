using Core.Entities.Vision.FileSettings.Models.DTO;
using Core.Entities.Vision.FileSettings.Models.UploadFileSettings;
using Core.Entities.Vision.FileSettings.Services;
using Core.Shared.Attributes;
using Core.Shared.Models.HttpResponse;
using Core.Shared.Services.System.Logs;
using Microsoft.AspNetCore.Mvc;
using BadHttpRequestException = Microsoft.AspNetCore.Http.BadHttpRequestException;

namespace ApiVision.Controllers;

[ApiController]
[Route("apiVision/fileSettings")]
[ServerAction]
public class FileSettingController : ControllerBase
{
	private readonly ILogService _logService;
	private readonly IFileSettingService _fileSettingService;

	public FileSettingController(ILogService logService, IFileSettingService fileSettingService)
	{
		_logService = logService;
		_fileSettingService = fileSettingService;
	}

	[HttpPut]
	public async Task<IActionResult> UploadFileSetting([FromForm] UploadFileSetting uploadFileSetting)
	{
		DTOFileSetting dtoFileSetting;
		try
		{
			if (uploadFileSetting.File == null)
				throw new BadHttpRequestException("No file was given");
			dtoFileSetting = await _fileSettingService.ReceiveFile(uploadFileSetting, uploadFileSetting.File);
		}
		catch (Exception e)
		{
			return await new ApiResponseObject().ErrorResult(_logService, ControllerContext, e);
		}

		return await new ApiResponseObject(dtoFileSetting).SuccessResult(_logService, ControllerContext);
	}
}