using Core.Entities.Packets.Dictionaries;
using Core.Entities.Packets.Models.DB.Shootings;

namespace Core.Entities.Packets.Models.DTO.Shootings;

public partial class DTOShooting
{
	public DTOShooting()
	{
		Type = PacketType.Shooting;
	}

	public DTOShooting(Shooting shootings) : base(shootings)
	{
		Type = PacketType.Shooting;
		AnodeIDKey = shootings.AnodeIDKey;
		GlobalStationStatus = shootings.GlobalStationStatus;
		ProcedurePerformance = shootings.ProcedurePerformance;
		LedStatus = shootings.LedStatus;
		ShootingTS = shootings.ShootingTS;
	}

	public override Shooting ToModel()
	{
		return new(this);
	}
}