using Core.Entities.Vision.ToDos.Models.DTO.ToUnloads;
using Mapster;

namespace Core.Entities.Vision.ToDos.Models.DB.ToUnloads;

public partial class ToUnload
{
	public ToUnload()
	{
	}

	public override DTOToUnload ToDTO() => this.Adapt<DTOToUnload>();
}