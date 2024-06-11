using Core.Entities.DebugsModes.Models.DB;
using Core.Shared.Models.DTO.Kernel;
using Core.Shared.Models.DTO.Kernel.Interfaces;

namespace Core.Entities.DebugsModes.Models.DTO;

public partial class DTODebugMode : DTOBaseEntity, IDTO<DebugMode, DTODebugMode>
{
	public bool DebugModeEnabled { get; set; }
	public bool LogEnabled { get; set; }
	public string LogSeverity { get; set; } = string.Empty;
	public bool CsvExportEnabled { get; set; }
}