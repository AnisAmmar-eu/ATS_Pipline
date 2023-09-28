using Core.Entities.Packets.Models.DB.Furnaces;
using Core.Shared.Models.DTOs.Kernel.Interfaces;

namespace Core.Entities.Packets.Models.DTO.Furnaces;

public partial class DTOFurnace : DTOPacket, IDTO<Furnace, DTOFurnace>
{
	public DTOFurnace(Furnace furnace) : base(furnace)
	{
	}
}