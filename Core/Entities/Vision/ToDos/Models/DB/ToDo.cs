using Core.Entities.Anodes.Dictionaries;
using Core.Entities.StationCycles.Models.DB;
using Core.Entities.Vision.ToDos.Dictionaries;
using Core.Entities.Vision.ToDos.Models.DTO;
using Core.Shared.Models.DB.Kernel;
using Core.Shared.Models.DB.Kernel.Interfaces;

namespace Core.Entities.Vision.ToDos.Models.DB;

public partial class ToDo : BaseEntity, IBaseEntity<ToDo, DTOToDo>
{
	public string CycleRID { get; set; }
	public int CameraID { get; set; }
	public int StationID { get; set; }
	public string AnodeType { get; set; }
	public DateTimeOffset? ShootingTS { get; set; }

    public int CycleID { get; set; }

    public StationCycle StationCycle
    {
        set => _StationCycle = value;
        get => _StationCycle
            ?? throw new InvalidOperationException("Uninitialized property: " + nameof(StationCycle));
    }

    private StationCycle? _StationCycle;
}