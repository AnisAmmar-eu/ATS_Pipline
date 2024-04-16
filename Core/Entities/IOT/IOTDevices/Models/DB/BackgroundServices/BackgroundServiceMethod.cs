using Core.Entities.IOT.IOTDevices.Models.DTO.BackgroundServices;
using Mapster;

namespace Core.Entities.IOT.IOTDevices.Models.DB.BackgroundServices;

public partial class BackgroundService
{
	public BackgroundService()
	{
	}

	public override DTOBackgroundService ToDTO() => this.Adapt<DTOBackgroundService>();
}