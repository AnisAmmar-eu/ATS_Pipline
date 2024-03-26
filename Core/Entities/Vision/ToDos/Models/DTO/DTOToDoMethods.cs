using Core.Entities.Vision.ToDos.Models.DB;

namespace Core.Entities.Vision.ToDos.Models.DTO;

public partial class DTOToDo
{
	public DTOToDo()
	{
	}

	public DTOToDo(ToDo todo) : base(todo)
	{
		CycleID = todo.CycleID;
		CycleRID = todo.CycleRID;
		CameraID = todo.CameraID;
		StationID = todo.StationID;
		AnodeType = todo.AnodeType;
		ShootingTS = todo.ShootingTS;
	}

	public override ToDo ToModel()
	{
		return new(this);
	}
}