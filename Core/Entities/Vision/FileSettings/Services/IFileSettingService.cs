using Core.Entities.Vision.FileSettings.Models.DB;
using Core.Entities.Vision.FileSettings.Models.DTO;
using Core.Entities.Vision.FileSettings.Models.UploadFileSettings;
using Core.Shared.Services.Kernel.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Core.Entities.Vision.FileSettings.Services;

public interface IFileSettingService : IBaseEntityService<FileSetting, DTOFileSetting>
{
	public Task<DTOFileSetting> ReceiveFile(UploadFileSetting uploadFileSetting, IFormFile file);
}