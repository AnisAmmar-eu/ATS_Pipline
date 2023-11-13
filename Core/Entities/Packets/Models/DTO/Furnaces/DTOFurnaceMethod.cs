using Core.Entities.Packets.Models.DB.Furnaces;

namespace Core.Entities.Packets.Models.DTO.Furnaces;

public partial class DTOFurnace
{
	public DTOFurnace()
	{
	}

	public DTOFurnace(Furnace furnace) : base(furnace)
	{
	}
}