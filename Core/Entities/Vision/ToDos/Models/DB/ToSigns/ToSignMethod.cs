using Core.Entities.Anodes.Dictionaries;
using Core.Entities.Packets.Models.DB.Shootings;
using Core.Entities.Vision.Dictionaries;
using Core.Entities.Vision.ToDos.Models.DTO.ToSigns;
using Core.Shared.Dictionaries;
using Mapster;

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
			.Map(dest => dest.StationCycleID, src => src.StationCycle!.ID)
			.Map(dest => dest.StationID, src => src.StationCycle!.StationID)
			.Map(dest => dest.CameraID, src => (src.Cam01Status == 1) ? 1 : (src.Cam02Status == 1) ? 2 : 0);

		return shooting.Adapt<ToSign>();
	}

	public List<InstanceMatchID> GetLoadDestinations()
	{
		List<InstanceMatchID> destinations = new();

		if (!Station.IsMatchStation(StationID))
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