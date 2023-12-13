using Core.Entities.Packets.Dictionaries;
using Core.Entities.Packets.Models.DB.Shootings;

namespace Core.Entities.Packets.Models.DTO.Shootings;

public partial class DTOShooting
{
	public DTOShooting()
	{
		Type = PacketType.Shooting;
	}

	public DTOShooting(Shooting shooting) : base(shooting)
	{
		Type = PacketType.Shooting;
		AnodeType = shooting.AnodeType;
		AnodeIDKey = shooting.AnodeIDKey;
		GlobalStationStatus = shooting.GlobalStationStatus;
		ProcedurePerformance = shooting.ProcedurePerformance;
		LedStatus = shooting.LedStatus;
		ShootingTS = shooting.ShootingTS;
	}

	public override Shooting ToModel()
	{
		return new(this);
	}
}