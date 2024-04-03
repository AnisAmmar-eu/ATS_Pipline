using Core.Entities.Anodes.Dictionaries;
using Core.Entities.Anodes.Models.DB;
using Core.Entities.Packets.Models.DB.Shootings;
using Core.Entities.Vision.Dictionaries;
using Core.Entities.Vision.ToDos.Models.DTO.ToSigns;
using Core.Migrations;
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

	public static ToSign ShootingToSign(Shooting shooting)
	{
		TypeAdapterConfig<Shooting, ToSign>.NewConfig()
			.Map(dest => dest.CycleRID, src => src.StationCycleRID)
			.Map(dest => dest.CycleID, src => src.StationCycle!.ID)
			.Map(dest => dest.StationID, src => src.StationCycle!.StationID)
			.Map(dest => dest.CameraID, src => (src.Cam01Status == 1) ? 1 : (src.Cam02Status == 1) ? 2 : 0);

		return shooting.Adapt<ToSign>();
	}

	public bool IsMatchStation()
	{
		return StationID >= Station.StationNameToID(Station.Station3);
	}

	public List<InstanceMatchID> GetLoadDestinations()
	{
		List<InstanceMatchID> destinations = new();

		if (!IsMatchStation())
		{
			destinations.Add(InstanceMatchID.S3);
			destinations.Add(InstanceMatchID.S4);
		}

		if (AnodeType.Equals(AnodeTypes.DX))
		{
			if (StationID == 3 || StationID == 4)
				destinations.Add(InstanceMatchID.S5);
			else if (StationID == 1 || StationID == 2)
				destinations.Add(InstanceMatchID.S5_C);
		}

		return destinations;
	}
}