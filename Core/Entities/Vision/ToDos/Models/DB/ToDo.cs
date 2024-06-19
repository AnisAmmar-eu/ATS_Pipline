using Core.Entities.Vision.ToDos.Models.DTO;
using Core.Shared.Models.DB.Kernel;
using Core.Shared.Models.DB.Kernel.Interfaces;

namespace Core.Entities.Vision.ToDos.Models.DB;

public partial class ToDo : BaseEntity, IBaseEntity<ToDo, DTOToDo>
{
	public string CycleRID { get; set; }
	public int StationID { get; set; }
	public string AnodeType { get; set; }
	public DateTimeOffset? ShootingTS { get; set; }
	public bool HasPlug { get; set; }
	public int NbActiveAlarms { get; set; }

	public int StationCycleID { get; set; }
}