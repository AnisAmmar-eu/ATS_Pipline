using Core.Entities.Packets.Models.DTO.Furnaces;

namespace Core.Entities.Packets.Models.DB.Furnaces;

public abstract partial class Furnace
{
	protected Furnace()
	{
	}

	protected Furnace(DTOFurnace dtoFurnace) : base(dtoFurnace)
	{
	}

	public override DTOFurnace ToDTO()
	{
		return new(this);
	}
}