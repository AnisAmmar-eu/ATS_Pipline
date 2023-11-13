using System.Runtime.InteropServices.ComTypes;
using Core.Entities.Vision.FileSettings.Models.DB;
using Core.Shared.Models.DTO.Kernel;
using Core.Shared.Models.DTO.Kernel.Interfaces;

namespace Core.Entities.Vision.FileSettings.Models.DTO;

public partial class DTOFileSetting : DTOBaseEntity, IDTO<FileSetting, DTOFileSetting>
{
	public string RID { get; set; } = string.Empty;
	public string FilePath { get; set; } = string.Empty;
	public DateTimeOffset LastModification { get; set; }
	public string LastUsername { get; set; } = string.Empty;
	public string LastUploadName { get; set; } = string.Empty;
	public string LastComment { get; set; } = string.Empty;
}