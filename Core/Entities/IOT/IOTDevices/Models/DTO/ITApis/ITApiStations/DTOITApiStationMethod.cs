using Core.Entities.IOT.IOTDevices.Models.DB.ITApiStations;

namespace Core.Entities.IOT.IOTDevices.Models.DTO.ITApiStations;

public partial class DTOITApiStation
{
	public DTOITApiStation()
	{
	}

	public DTOITApiStation(ITApiStation itApi) : base(itApi)
	{
	}

	public override ITApiStation ToModel()
	{
		return new(this);
	}
}