using Core.Entities.Vision.ToDos.Models.DTO.ToLoads;
using Mapster;

namespace Core.Entities.Vision.ToDos.Models.DB.ToLoads;

public partial class ToLoad
{
	public ToLoad()
	{
	}

	public override DTOToLoad ToDTO() => this.Adapt<DTOToLoad>();
}