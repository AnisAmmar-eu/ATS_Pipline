using Core.Entities.Packets.Models.DB.Shootings;
using Core.Entities.StationCycles.Models.DB;
using Core.Entities.Vision.ToDos.Models.DTO.ToSigns;
using Mapster;

namespace Core.Entities.Vision.ToDos.Models.DB.ToSigns;

public partial class ToSign
{
	public ToSign()
	{
	}

	public override DTOToSign ToDTO() => this.Adapt<DTOToSign>();

	public static ToSign ShootingToSign(Shooting shooting, StationCycle stationCycle)
	{
		TypeAdapterConfig<Shooting, ToSign>.NewConfig()
			.Map(dest => dest.CycleRID, src => src.StationCycleRID)
			.Map(dest => dest.StationCycleID, _ => stationCycle.ID)
			.Map(dest => dest.StationID, _ => stationCycle.StationID)
			.Map(dest => dest.CameraID, src => (src.Cam01Status == 1) ? 1 : (src.Cam02Status == 1) ? 2 : 0);

		return shooting.Adapt<ToSign>();
	}
}