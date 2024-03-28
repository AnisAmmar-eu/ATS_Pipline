using Core.Entities.Vision.ToDos.Models.DTO.ToSigns;
using Mapster;

namespace Core.Entities.Vision.ToDos.Models.DB.ToSigns;

public partial class ToSign
{
	public ToSign()
	{
	}

	public override DTOToSign ToDTO()
	{
		return this.Adapt<DTOToSign>();
	}
}