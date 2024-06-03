using Core.Entities.StationCycles.Models.DB;
using Core.Entities.Vision.ToDos.Models.DTO.ToMatchs;
using Mapster;

namespace Core.Entities.Vision.ToDos.Models.DB.ToMatchs;

public partial class ToMatch
{
	public ToMatch()
	{
	}

	public ToMatch(StationCycle stationCycle, int instanceMatchID)
	{
		CycleRID = stationCycle.RID;
		StationID = stationCycle.StationID;
		AnodeType = stationCycle.AnodeType;
		ShootingTS = stationCycle.TSFirstShooting;
		StationCycleID = stationCycle.ID;
		InstanceMatchID = instanceMatchID;
		HasPlug = stationCycle.HasPlug;
		NbActiveAlarms = stationCycle.NbActiveAlarms;
	}

	public override DTOToMatch ToDTO() => this.Adapt<DTOToMatch>();
}