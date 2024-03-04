using Core.Entities.Anodes.Models.DTO;
using Core.Entities.Packets.Dictionaries;
using Core.Entities.StationCycles.Models.DB.LoadableCycles.S1S2Cycles;
using Core.Entities.StationCycles.Models.DB.MatchableCycles.S3S4Cycles;
using Core.Shared.Models.DB.Kernel;
using Core.Shared.Models.DB.Kernel.Interfaces;

namespace Core.Entities.Anodes.Models.DB;

public abstract partial class Anode : BaseEntity, IBaseEntity<Anode, DTOAnode>, IBaseKPI<Anode>
{
	public string CycleRID { get; set; } = string.Empty;
	public PacketStatus Status { get; set; } = PacketStatus.Initialized;
	public DateTimeOffset? ClosedTS { get; set; }

	public int? S1S2CycleID { get; set; }
	public int? S1S2CycleStationID { get; set; }
	public DateTimeOffset? S1S2CycleTS { get; set; }
	public S1S2Cycle? S1S2Cycle { get; set; }

	public int? S3S4CycleID { get; set; }
	public int? S3S4CycleStationID { get; set; }
	public DateTimeOffset? S3S4CycleTS { get; set; }
	public S3S4Cycle? S3S4Cycle { get; set; }
}