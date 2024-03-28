using Core.Entities.Vision.ToDos.Models.DB.ToSigns;
using Core.Shared.Models.DTO.Kernel.Interfaces;
using Mapster;

namespace Core.Entities.Vision.ToDos.Models.DTO.ToSigns;

public partial class DTOToSign
{
	public DTOToSign()
	{
	}

	public override ToSign ToModel()
	{
		return this.Adapt<ToSign>();
	}
}