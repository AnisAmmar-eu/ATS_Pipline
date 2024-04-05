using Core.Entities.Vision.ToDos.Models.DB.ToNotifys;
using Mapster;

namespace Core.Entities.Vision.ToDos.Models.DTO.ToNotifys;

public partial class DTOToNotify
{
	public DTOToNotify()
	{
	}

	public override ToNotify ToModel()
	{
		return this.Adapt<ToNotify>();
	}
}