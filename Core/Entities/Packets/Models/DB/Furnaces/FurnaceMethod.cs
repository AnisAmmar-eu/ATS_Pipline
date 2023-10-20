using Core.Entities.Packets.Models.DTO.Furnaces;
using Core.Entities.Packets.Models.DTO.Furnaces.InFurnaces;
using Core.Shared.Models.DB.Kernel.Interfaces;

namespace Core.Entities.Packets.Models.DB.Furnaces;

public abstract partial class Furnace : Packet, IBaseEntity<Furnace, DTOFurnace>
{
	protected Furnace()
	{}
	protected Furnace(DTOFurnace dtoFurnace) : base(dtoFurnace)
	{}
	public override DTOFurnace ToDTO()
	{
		return new DTOFurnace(this);
	}
}