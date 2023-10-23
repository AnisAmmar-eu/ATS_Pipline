using Core.Entities.Packets.Dictionaries;
using Core.Entities.Packets.Models.DB;
using Core.Entities.Packets.Models.DB.Furnaces;
using Core.Entities.Packets.Models.DTO.Furnaces.InFurnaces;
using Core.Shared.Models.DTO.Kernel.Interfaces;

namespace Core.Entities.Packets.Models.DTO.Furnaces;

public partial class DTOFurnace : DTOPacket, IDTO<Furnace, DTOFurnace>
{
	public DTOFurnace() : base()
	{
		Type = PacketType.Furnace;
	}

	public DTOFurnace(Furnace furnace) : base(furnace)
	{
		Type = PacketType.Furnace;
	}
}