using Core.Entities.Vision.ToDos.Models.DB;
using Core.Shared.Models.DTO.Kernel;
using Core.Shared.Models.DTO.Kernel.Interfaces;

namespace Core.Entities.Vision.ToDos.Models.DTO;

public partial class DTOToDo : DTOBaseEntity, IDTO<ToDo, DTOToDo>
{
	public int CycleID { get; set; }
	public string CycleRID { get; set; }
	public int StationID { get; set; }
	public string AnodeType { get; set; }
	public DateTimeOffset? ShootingTS { get; set; }
	public bool HasPlug { get; set; }
	public int NbActiveAlarms { get; set; }
}