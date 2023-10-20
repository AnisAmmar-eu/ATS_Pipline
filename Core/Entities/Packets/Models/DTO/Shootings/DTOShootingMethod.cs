using Core.Entities.Packets.Dictionaries;
using Core.Entities.Packets.Models.DB.Shootings;
using Core.Shared.Models.DTO.Kernel.Interfaces;

namespace Core.Entities.Packets.Models.DTO.Shootings;

public partial class DTOShooting : DTOPacket, IDTO<Shooting, DTOShooting>
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
		StationCycle = shootings.StationCycle;
	}

	public override Shooting ToModel()
	{
		return new Shooting(this);
	}
}