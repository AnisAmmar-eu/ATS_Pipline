using Core.Entities.IOT.IOTDevices.Models.DB.Stations;

namespace Core.Entities.IOT.IOTDevices.Models.DTO.Stations;

public partial class DTOStation
{
	public DTOStation()
	{
	}

	public DTOStation(Station Station) : base(Station)
	{
	}

	public override Station ToModel()
	{
		return new(this);
	}
}