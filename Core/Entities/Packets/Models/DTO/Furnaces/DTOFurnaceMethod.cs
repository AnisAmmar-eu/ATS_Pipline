using Core.Entities.Packets.Models.DB.Furnaces;
using Core.Entities.Packets.Models.DTO.Furnaces.InFurnaces;
using Core.Entities.Packets.Models.DTO.Furnaces.OutFurnaces;

namespace Core.Entities.Packets.Models.DTO.Furnaces;

public partial class DTOFurnace
{
	public DTOFurnace()
	{
	}

	public DTOFurnace(Furnace furnace) : base(furnace)
	{
	}

	public override Furnace ToModel()
	{
		return this switch {
			DTOInFurnace dtoInFurnace => dtoInFurnace.ToModel(),
			DTOOutFurnace dtoOutFurnace => dtoOutFurnace.ToModel(),
			_ => throw new InvalidCastException("Trying to convert an abstract class to model"),
		};
	}
}