using Core.Entities.DebugsModes.Models.DTO;
using Core.Shared.Models.DB.Kernel;
using Core.Shared.Models.DB.Kernel.Interfaces;

namespace Core.Entities.DebugsModes.Models.DB;

public partial class DebugMode : BaseEntity, IBaseEntity<DebugMode, DTODebugMode>
{
	public bool DebugModeEnabled { get; set; }
	public bool LogEnabled { get; set; }
	public string LogSeverity { get; set; }
	public bool CsvExportEnabled { get; set; }
}