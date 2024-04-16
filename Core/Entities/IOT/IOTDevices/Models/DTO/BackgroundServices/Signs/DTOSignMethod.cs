using Core.Entities.IOT.IOTDevices.Models.DB.BackgroundServices.Signs;
using Mapster;

namespace Core.Entities.IOT.IOTDevices.Models.DTO.BackgroundServices.Signs;

public partial class DTOSign
{
	public DTOSign()
	{
	}

	public override Sign ToModel() => this.Adapt<Sign>();
}