using Core.Entities.Packets.Dictionaries;
using Core.Entities.Packets.Models.DB.Shootings;
using Mapster;

namespace Core.Entities.Packets.Models.DTO.Shootings;

public partial class DTOShooting
{
	public DTOShooting()
	{
		Type = PacketTypes.Shooting;
	}

	public override Shooting ToModel()
	{
		Shooting shooting = this.Adapt<Shooting>();
		Type = PacketTypes.Shooting;
		return shooting;
	}
}