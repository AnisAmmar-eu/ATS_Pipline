using Core.Entities.Vision.ToDos.Models.DTO.ToUnloads;

namespace Core.Entities.Vision.ToDos.Models.DB.ToUnloads;

public partial class ToUnload
{
	public ToUnload()
	{
	}

	public ToUnload(DTOToUnload dtoUnload) : base(dtoUnload)
	{
		SANfile = dtoUnload.SANfile;
	}

	public override DTOToUnload ToDTO()
	{
		return new(this);
	}
}