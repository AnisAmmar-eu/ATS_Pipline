using Core.Entities.Vision.FileSettings.Models.DTO;
using Core.Shared.Models.DB.Kernel;
using Core.Shared.Models.DB.Kernel.Interfaces;

namespace Core.Entities.Vision.FileSettings.Models.DB;

public partial class FileSetting : BaseEntity, IBaseEntity<FileSetting, DTOFileSetting>
{
	public string RID { get; set; } = string.Empty;
	public string FilePath { get; set; } = string.Empty;
	public DateTimeOffset LastModification { get; set; }
	public string LastUsername { get; set; } = string.Empty;
	public string LastUploadName { get; set; } = string.Empty;
	public string LastComment { get; set; } = string.Empty;
}