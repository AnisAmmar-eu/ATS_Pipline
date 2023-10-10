using Core.Entities.Packets.Models.DB.Shootings;
using Core.Shared.Models.DTO.Kernel.Interfaces;

namespace Core.Entities.Packets.Models.DTO.Shootings;

public partial class DTOShooting : DTOPacket, IDTO<Shooting, DTOShooting>
{
	public DTOShooting()
	{
		Type = "Shooting";
	}

	public DTOShooting(Shooting shootings) : base(shootings)
	{
		AnodeIDKey = shootings.AnodeIDKey;
		GlobalStationStatus = shootings.GlobalStationStatus;
		ProcedurePerformance = shootings.ProcedurePerformance;
		LedStatus = shootings.LedStatus;
		ShootingTS = shootings.ShootingTS;
	}

	public override Shooting ToModel()
	{
		return new Shooting(this);
	}
}