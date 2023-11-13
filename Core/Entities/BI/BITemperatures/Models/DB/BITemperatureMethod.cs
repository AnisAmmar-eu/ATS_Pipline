using System.Globalization;
using Core.Entities.BI.BITemperatures.Models.DTO;
using Core.Entities.IOT.IOTTags.Models.DB;
using Core.Shared.Dictionaries;

namespace Core.Entities.BI.BITemperatures.Models.DB;

public partial class BITemperature
{
	public BITemperature()
	{
	}

	public BITemperature(IOTTag cameraTemperature)
	{
		TS = DateTimeOffset.Now;
		TemperatureRID = cameraTemperature.RID;
		StationID = Station.ID;
		Temperature = double.Parse(cameraTemperature.CurrentValue, CultureInfo.InvariantCulture);
	}

	public override DTOBITemperature ToDTO()
	{
		return new DTOBITemperature(this);
	}
}