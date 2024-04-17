using Core.Entities.Vision.ToDos.Models.DB.ToSigns;
using Mapster;

namespace Core.Entities.Vision.ToDos.Models.DTO.ToSigns;

public partial class DTOToSign
{
	public DTOToSign()
	{
	}

	public override ToSign ToModel() => this.Adapt<ToSign>();
}