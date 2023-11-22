using Carter;
using Core.Entities.Vision.FileSettings.Models.DB;
using Core.Entities.Vision.FileSettings.Models.DTO;
using Core.Entities.Vision.FileSettings.Models.UploadFileSettings;
using Core.Entities.Vision.FileSettings.Services;
using Core.Shared.Dictionaries;
using Core.Shared.Endpoints.Kernel;
using Core.Shared.Models.ApiResponses;
using Core.Shared.Services.System.Logs;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ApiVision.Controllers;

public class FileSettingController : BaseEndpoint<FileSetting, DTOFileSetting, IFileSettingService>, ICarterModule
{
	public void AddRoutes(IEndpointRouteBuilder app)
	{
		if (!Station.IsServer)
			return;
		app.MapGroup("apiVision/fileSettings").WithTags(nameof(FileSettingController)).MapPut("", UploadFileSetting);
	}

	private static async Task<JsonHttpResult<ApiResponse>> UploadFileSetting(
		[FromForm] UploadFileSetting uploadFileSetting, IFileSettingService fileSettingService, ILogService logService,
		HttpContext httpContext)
	{
		return await GenericController(async () =>
		{
			if (uploadFileSetting.File == null)
				throw new BadHttpRequestException("No file was given");
			return await fileSettingService.ReceiveFile(uploadFileSetting, uploadFileSetting.File);
		}, logService, httpContext);
	}
}