using Core.Entities.IOT.IOTDevices.Models.DB.BackgroundServices;
using Mapster;

namespace Core.Entities.IOT.IOTDevices.Models.DTO.BackgroundServices;

public partial class DTOBackgroundService
{
	public DTOBackgroundService()
	{
	}

	public override BackgroundService ToModel() => this.Adapt<BackgroundService>();
}