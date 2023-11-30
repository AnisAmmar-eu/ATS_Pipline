using Microsoft.AspNetCore.Http;

namespace Core.Entities.Vision.FileSettings.Models.UploadFileSettings;

public class UploadFileSetting
{
	public string RID { get; set; } = string.Empty;
	public string Username { get; set; } = string.Empty;
	public string Comment { get; set; } = string.Empty;
    public IFormFile? File { get; set; }
}