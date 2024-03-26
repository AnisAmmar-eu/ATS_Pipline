using Core.Entities.Vision.ToDos.Models.DTO.ToNotifys;

namespace Core.Entities.Vision.ToDos.Models.DB.ToNotifys;

public partial class ToNotify
{
	public ToNotify()
	{
	}

	public ToNotify(DTOToNotify dtoNotify) : base(dtoNotify)
	{
		SynchronisationKey = dtoNotify.SynchronisationKey;
	}

	public override DTOToNotify ToDTO()
	{
		return new(this);
	}
}