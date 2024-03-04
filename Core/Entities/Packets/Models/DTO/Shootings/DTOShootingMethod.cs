using Core.Entities.Packets.Dictionaries;
using Core.Entities.Packets.Models.DB.Shootings;

namespace Core.Entities.Packets.Models.DTO.Shootings;

public partial class DTOShooting
{
	public DTOShooting()
	{
		Type = PacketTypes.Shooting;
	}

	public DTOShooting(Shooting shooting) : base(shooting)
	{
		Type = PacketTypes.Shooting;
		ShootingTS = shooting.ShootingTS;
		AnodeType = shooting.AnodeType;
		Cam01Status = shooting.Cam01Status;
		Cam02Status = shooting.Cam02Status;
	}

	public override Shooting ToModel()
	{
		return new(this);
	}
}