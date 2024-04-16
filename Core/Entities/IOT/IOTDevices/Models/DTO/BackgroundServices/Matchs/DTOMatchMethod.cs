using Core.Entities.IOT.IOTDevices.Models.DB.BackgroundServices.Matchs;
using Mapster;

namespace Core.Entities.IOT.IOTDevices.Models.DTO.BackgroundServices.Matchs;

public partial class DTOMatch
{
	public DTOMatch()
	{
	}

	public override Match ToModel() => this.Adapt<Match>();
}