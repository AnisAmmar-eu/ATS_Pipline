using Core.Entities.Anodes.Models.DB;
using Core.Entities.StationCycles.Models.DB;
using Core.Entities.StationCycles.Models.DB.LoadableCycles.S1S2Cycles;
using Core.Entities.Vision.ToDos.Models.DB.ToSigns;
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