using Core.Entities.Vision.ToDo.Models.DB.ToUnloads;
using Core.Shared.Models.DTO.Kernel.Interfaces;

namespace Core.Entities.Vision.ToDo.Models.DTO.ToUnloads;

public partial class DTOToUnload
{
	public DTOToUnload()
	{
	}

	public DTOToUnload(ToUnload dtoUnload) : base(dtoUnload)
	{
		SynchronisationKey = dtoUnload.SynchronisationKey;
	}

	public override ToUnload ToModel()
	{
		return new(this);
	}
}