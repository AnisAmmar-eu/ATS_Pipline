using Core.Entities.Vision.ToDo.Models.DTO.ToUnloads;

namespace Core.Entities.Vision.ToDo.Models.DB.ToUnloads;

public partial class ToUnload
{
	public ToUnload()
	{
	}

	public ToUnload(DTOToUnload dtoUnload) : base(dtoUnload)
	{
		SynchronisationKey = dtoUnload.SynchronisationKey;
	}

	public override DTOToUnload ToDTO()
	{
		return new(this);
	}
}