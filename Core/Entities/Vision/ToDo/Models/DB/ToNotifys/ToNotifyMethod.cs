using Core.Entities.Vision.ToDo.Models.DTO.ToNotifys;

namespace Core.Entities.Vision.ToDo.Models.DB.ToNotifys;

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