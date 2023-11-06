using Core.Entities.Anodes.Models.DB.AnodesD20;
using Core.Entities.Anodes.Models.DB.AnodesDX;
using Core.Entities.Anodes.Models.DTO;
using Core.Entities.StationCycles.Models.DB;
using Core.Shared.Dictionaries;
using Core.Shared.Models.DB.Kernel;
using Core.Shared.Models.DB.Kernel.Interfaces;

namespace Core.Entities.Anodes.Models.DB;

public abstract partial class Anode : BaseEntity, IBaseEntity<Anode, DTOAnode>
{
	public override DTOAnode ToDTO()
	{
		return new DTOAnode(this);
	}

	public static Anode Create(StationCycle stationCycle)
	{
		if (stationCycle.AnodeType == AnodeTypeDict.D20)
			return new AnodeD20();
		return new AnodeDX();
	}
}