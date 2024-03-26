using Core.Entities.Vision.ToDos.Models.DB.ToUnloads;
using Core.Shared.Models.DTO.Kernel.Interfaces;

namespace Core.Entities.Vision.ToDos.Models.DTO.ToUnloads;

public partial class DTOToUnload
{
	public DTOToUnload()
	{
	}

	public DTOToUnload(ToUnload dtoUnload) : base(dtoUnload)
	{
		SANfile = dtoUnload.SANfile;
	}

	public override ToUnload ToModel()
	{
		return new(this);
	}
}