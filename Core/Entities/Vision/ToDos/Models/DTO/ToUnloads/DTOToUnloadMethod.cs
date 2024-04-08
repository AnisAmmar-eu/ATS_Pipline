using Core.Entities.Vision.ToDos.Models.DB.ToUnloads;
using Mapster;

namespace Core.Entities.Vision.ToDos.Models.DTO.ToUnloads;

public partial class DTOToUnload
{
	public DTOToUnload()
	{
	}

	public override ToUnload ToModel()
	{
		return this.Adapt<ToUnload>();
	}
}