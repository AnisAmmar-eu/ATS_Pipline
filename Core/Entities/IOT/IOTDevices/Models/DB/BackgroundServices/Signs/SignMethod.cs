using Core.Entities.IOT.IOTDevices.Models.DTO.BackgroundServices.Signs;
using Mapster;

namespace Core.Entities.IOT.IOTDevices.Models.DB.BackgroundServices.Signs;

public partial class Sign
{
	public Sign()
	{
	}

	public override DTOSign ToDTO() => this.Adapt<DTOSign>();
}