using Core.Entities.Vision.ToDos.Models.DTO;
using Core.Entities.Vision.ToDos.Models.DTO.ToSigns;
using Mapster;

namespace Core.Entities.Vision.ToDos.Models.DB;

public partial class ToDo
{
	public ToDo()
	{
	}

	public override DTOToDo ToDTO()
	{
		return this.Adapt<DTOToSign>();
	}
}