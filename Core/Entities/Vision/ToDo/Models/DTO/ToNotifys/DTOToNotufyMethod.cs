using Core.Entities.Vision.ToDo.Models.DB.ToNotifys;
using Core.Shared.Models.DTO.Kernel.Interfaces;

namespace Core.Entities.Vision.ToDo.Models.DTO.ToNotifys;

public partial class DTOToNotify
{
	public DTOToNotify()
	{
	}

	public DTOToNotify(ToNotify dtoNotify) : base(dtoNotify)
	{
		SynchronisationKey = dtoNotify.SynchronisationKey;
	}

	public override ToNotify ToModel()
	{
		return new(this);
	}
}