using Core.Entities.Vision.ToDos.Models.DTO;

namespace Core.Entities.Vision.ToDos.Models.DB;

public partial class ToDo
{
	public ToDo()
	{
	}

	public ToDo(DTOToDo dtoToDo) : base(dtoToDo)
	{
		CycleID = dtoToDo.CycleID;
		CycleRID = dtoToDo.CycleRID;
		CameraID = dtoToDo.CameraID;
		StationID = dtoToDo.StationID;
		AnodeType = dtoToDo.AnodeType;
		ShootingTS = dtoToDo.ShootingTS;
	}

	public override DTOToDo ToDTO()
	{
		return new(this);
	}
}