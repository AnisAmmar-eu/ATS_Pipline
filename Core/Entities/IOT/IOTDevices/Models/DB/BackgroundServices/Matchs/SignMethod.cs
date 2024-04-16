using Core.Entities.IOT.IOTDevices.Models.DTO.BackgroundServices.Matchs;
using Mapster;

namespace Core.Entities.IOT.IOTDevices.Models.DB.BackgroundServices.Matchs;

public partial class Match
{
	public Match()
	{
	}

	public override DTOMatch ToDTO() => this.Adapt<DTOMatch>();
}