using Core.Entities.Packets.Models.DTO.Shootings;
using Core.Shared.Models.DB.Kernel.Interfaces;

namespace Core.Entities.Packets.Models.DB.Shootings;

public partial class Shooting : Packet, IBaseEntity<Shooting, DTOShooting>
{
	public override DTOShooting ToDTO(string? languageRID = null)
	{
		return new DTOShooting(this);
	}
}