using Core.Entities.Vision.ToDos.Models.DB.ToLoads;
using Core.Shared.Models.DTO.Kernel.Interfaces;
using Mapster;

namespace Core.Entities.Vision.ToDos.Models.DTO.ToLoads;

public partial class DTOToLoad : DTOToDo, IDTO<ToLoad, DTOToLoad>
{
	public DTOToLoad()
	{
	}

	public override ToLoad ToModel() => this.Adapt<ToLoad>();
}