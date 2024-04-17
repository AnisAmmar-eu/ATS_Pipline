using Core.Entities.BI.BITemperatures.Models.DB;

namespace Core.Entities.BI.BITemperatures.Models.DTO;

public partial class DTOBITemperature
{
	public DTOBITemperature()
	{
	}

	public DTOBITemperature(BITemperature biTemperature)
	{
		ID = biTemperature.ID;
		TS = biTemperature.TS;
		StationID = biTemperature.StationID;
		TemperatureRID = biTemperature.TemperatureRID;
		Temperature = biTemperature.Temperature;
	}

	public override BITemperature ToModel() => new(this);
}