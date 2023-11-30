using Core.Entities.Vision.FileSettings.Dictionaries;
using Core.Entities.Vision.FileSettings.Models.DB;
using Core.Shared.Data;

namespace Core.Entities.Vision.Data;

public static class VisionInitializer
{
	public static void Initialize(AnodeCTX anodeCTX)
	{
		if (anodeCTX.FileSetting.Any())
			return;

		InitializeFileSetting(anodeCTX, FileSettingRID.S1SignCam1DXStatic, FileSettingPath.S1SignCam1DXStatic);
		InitializeFileSetting(anodeCTX, FileSettingRID.S1SignCam1D20Static, FileSettingPath.S1SignCam1D20Static);
		anodeCTX.SaveChanges();
	}

	private static void InitializeFileSetting(AnodeCTX anodeCTX, string rid, string filePath)
	{
		anodeCTX.FileSetting.Add(new FileSetting {
			RID = rid,
			FilePath = filePath,
			LastModification = DateTimeOffset.Now,
			LastUsername = "init",
			LastUploadName = string.Empty,
			LastComment = "init",
		});
	}
}