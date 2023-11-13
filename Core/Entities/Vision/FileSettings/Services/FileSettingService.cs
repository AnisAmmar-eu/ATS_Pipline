using System.Linq.Expressions;
using Core.Entities.Vision.FileSettings.Models.DB;
using Core.Entities.Vision.FileSettings.Models.DTO;
using Core.Entities.Vision.FileSettings.Models.UploadFileSettings;
using Core.Entities.Vision.FileSettings.Repositories;
using Core.Shared.Services.Kernel;
using Core.Shared.UnitOfWork.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Core.Entities.Vision.FileSettings.Services;

public class FileSettingService : ServiceBaseEntity<IFileSettingRepository, FileSetting, DTOFileSetting>, IFileSettingService
{
	public FileSettingService(IAnodeUOW anodeUOW) : base(anodeUOW)
	{
	}

	public async Task<DTOFileSetting> ReceiveFile(UploadFileSetting uploadFileSetting, IFormFile file)
	{
		FileSetting fileSetting = await AnodeUOW.FileSetting.GetBy(new Expression<Func<FileSetting, bool>>[]
		{
			setting => setting.RID == uploadFileSetting.RID
		}, withTracking: false);
		
		fileSetting.LastUsername = uploadFileSetting.Username;
		fileSetting.LastComment = uploadFileSetting.Comment;
		fileSetting.LastModification = DateTimeOffset.Now;
		fileSetting.LastUploadName = file.FileName;

		FileInfo oldFile = new(fileSetting.FilePath);
		Directory.CreateDirectory(oldFile.DirectoryName!);
		await using FileStream fileStream = oldFile.Open(FileMode.Create);
		await file.CopyToAsync(fileStream);
		await AnodeUOW.StartTransaction();
		AnodeUOW.FileSetting.Update(fileSetting);
		AnodeUOW.Commit();
		await AnodeUOW.CommitTransaction();
		return fileSetting.ToDTO();
	}
}