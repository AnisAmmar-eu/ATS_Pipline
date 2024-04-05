using Core.Entities.Vision.ToDos.Models.DB;
using Mapster;

namespace Core.Entities.Vision.ToDos.Models.DTO;

public partial class DTOToDo
{
	public DTOToDo()
	{
	}

	public override DB.ToDo ToModel()
	{
		return this.Adapt<DB.ToDo>();
	}
}