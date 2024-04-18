using Mapster;
using Core.Entities.Vision.ToDos.Models.DB;

namespace Core.Entities.Vision.ToDos.Models.DTO;

public partial class DTOToDo
{
	public DTOToDo()
	{
	}

	public override ToDo ToModel() => this.Adapt<ToDo>();
}