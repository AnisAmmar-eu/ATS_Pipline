using Core.Entities.Anodes.Dictionaries;
using Core.Entities.Vision.ToDos.Dictionaries;
using Core.Entities.Vision.ToDos.Models.DTO.ToSigns;
using Core.Shared.Dictionaries;
using Mapster;
using System.ComponentModel.Composition;

namespace Core.Entities.Vision.ToDos.Models.DB.ToSigns;

public partial class ToSign
{
	public ToSign()
	{
	}

	public override DTOToSign ToDTO()
	{
		return this.Adapt<DTOToSign>();
	}

	public bool IsMatchStation(int stationId)
	{
		return stationId >= Station.StationNameToID(Station.Station3);
	}

	public List<DataSetID> GetDestinations()
	{
		List<DataSetID> destinations = new();

		if (StationID == 1 || StationID == 2)
		{
			destinations.Add(DataSetID.S3);
			destinations.Add(DataSetID.S4);
		}

		if (AnodeType.Equals(AnodeTypes.DX))
		{
			if (StationID == 3 || StationID == 4)
				destinations.Add(DataSetID.S5);
			else if (StationID == 1 || StationID == 2)
				destinations.Add(DataSetID.S5_C);
		}

		return destinations;
	}
}