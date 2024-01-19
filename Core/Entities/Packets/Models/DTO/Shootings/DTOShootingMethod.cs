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
		ShootingTS = shooting.ShootingTS;
		SyncIndex = shooting.SyncIndex;
		AnodeType = shooting.AnodeType;
		AnodeSize = shooting.AnodeSize;
		Cam01Status = shooting.Cam01Status;
		Cam02Status = shooting.Cam02Status;
	}

	public override Shooting ToModel()
	{
		return new(this);
	}
}