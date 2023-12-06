using Core.Entities.Anodes.Models.DB.AnodesD20;
using Core.Entities.Anodes.Models.DB.AnodesDX;
using Core.Entities.Anodes.Models.DTO;
using Core.Entities.KPI.KPICs.Dictionaries;
using Core.Entities.StationCycles.Models.DB.MatchingCycles.S3S4Cycles;
using Core.Entities.StationCycles.Models.DB.SigningCycles.S1S2Cycles;
using Core.Shared.Dictionaries;

namespace Core.Entities.Anodes.Models.DB;

public abstract partial class Anode
{
	protected Anode()
	{
	}

	protected Anode(DTOAnode dtoAnode)
	{
		S1S2CycleRID = dtoAnode.S1S2CycleRID;
		Status = dtoAnode.Status;
		ClosedTS = dtoAnode.ClosedTS;

		S1S2CycleID = dtoAnode.S1S2CycleID;
		S1S2CycleStationID = dtoAnode.S1S2CycleStationID;
		S1S2CycleTS = dtoAnode.S1S2CycleTS;
		S1S2Cycle = dtoAnode.S1S2Cycle.ToModel();

		S3S4CycleID = dtoAnode.S3S4CycleID;
		S3S4CycleStationID = dtoAnode.S3S4CycleStationID;
		S3S4CycleTS = dtoAnode.S3S4CycleTS;
		S3S4Cycle = dtoAnode.S3S4Cycle?.ToModel();
	}

	protected Anode(S1S2Cycle cycle)
	{
		S1S2Cycle = cycle;
		S1S2CycleID = cycle.ID;
		S1S2CycleTS = cycle.TS;
		S1S2CycleStationID = cycle.StationID;
		S1S2CycleRID = cycle.RID;
	}

	public override DTOAnode ToDTO()
	{
		return this switch {
			AnodeD20 anodeD20 => anodeD20.ToDTO(),
			AnodeDX anodeDX => anodeDX.ToDTO(),
			_ => throw new InvalidCastException("Trying to convert an abstract class to DTO"),
		};
	}

	public Anode GetValue()
	{
		return this;
	}

	public static string[] GetKPICRID()
	{
		return [KPICRID.AnodesTotalNumber];
	}

	public Func<List<Anode>, string[]> GetComputedValues()
	{
		return anodes => [anodes.Count.ToString()];
	}

	public static Anode Create(S1S2Cycle cycle)
	{
		if (cycle.AnodeType == AnodeTypeDict.D20)
			return new AnodeD20(cycle);

		return new AnodeDX(cycle);
	}

	public void AddS3S4Cycle(S3S4Cycle cycle)
	{
		S3S4Cycle = cycle;
		S3S4CycleID = cycle.ID;
		S3S4CycleTS = cycle.TS;
		S3S4CycleStationID = cycle.StationID;
	}
}