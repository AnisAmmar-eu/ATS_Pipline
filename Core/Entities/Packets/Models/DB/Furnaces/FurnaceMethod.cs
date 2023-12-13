using System.Text;
using System.Text.Json;
using Core.Entities.Packets.Dictionaries;
using Core.Entities.Packets.Models.DTO.Furnaces;
using Core.Entities.StationCycles.Models.DB.MatchableCycles.S3S4Cycles;
using Core.Shared.Dictionaries;
using Core.Shared.UnitOfWork.Interfaces;

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