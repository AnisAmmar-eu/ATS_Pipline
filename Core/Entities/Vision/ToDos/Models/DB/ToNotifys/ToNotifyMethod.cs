using Core.Entities.Vision.ToDos.Models.DTO.ToNotifys;
using Mapster;

namespace Core.Entities.Vision.ToDos.Models.DB.ToNotifys;

public partial class ToNotify
{
	public ToNotify()
	{
	}

	public override DTOToNotify ToDTO()
	{
		return this.Adapt<DTOToNotify>();
	}
}