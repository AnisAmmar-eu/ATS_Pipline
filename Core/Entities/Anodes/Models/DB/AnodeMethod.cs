using Core.Entities.Anodes.Models.DB.AnodesD20;
using Core.Entities.Anodes.Models.DB.AnodesDX;
using Core.Entities.Anodes.Models.DTO;
using Core.Entities.KPI.KPICs.Dictionaries;
using Core.Entities.StationCycles.Models.DB;
using Core.Shared.Dictionaries;

namespace Core.Entities.Anodes.Models.DB;

public abstract partial class Anode
{
	public override DTOAnode ToDTO()
	{
		return new DTOAnode(this);
	}

	public Anode GetValue()
	{
		return this;
	}

	public string[] GetKPICRID()
	{
		return new[] { KPICRID.AnodesTotalNumber };
	}

	public Func<List<Anode>, string[]> GetComputedValues()
	{
		return anodes => { return new[] { anodes.Count.ToString() }; };
	}

	public static Anode Create(StationCycle stationCycle)
	{
		if (stationCycle.AnodeType == AnodeTypeDict.D20)
			return new AnodeD20();
		return new AnodeDX();
	}
}