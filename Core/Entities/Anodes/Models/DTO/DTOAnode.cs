using Core.Entities.Anodes.Models.DB;
using Core.Entities.Packets.Dictionaries;
using Core.Entities.StationCycles.Models.DTO.MatchingCycles.S3S4Cycles;
using Core.Entities.StationCycles.Models.DTO.SigningCycles.S1S2Cycles;
using Core.Shared.Models.DTO.Kernel;
using Core.Shared.Models.DTO.Kernel.Interfaces;

namespace Core.Entities.Anodes.Models.DTO;

public partial class DTOAnode : DTOBaseEntity, IDTO<Anode, DTOAnode>
{
	public string S1S2CycleRID { get; set; } = string.Empty;
	public string Status { get; set; } = PacketStatus.Initialized;
	public DateTimeOffset? ClosedTS { get; set; }

	public int S1S2CycleID { get; set; }
	public int S1S2CycleStationID { get; set; }
	public DateTimeOffset S1S2CycleTS { get; set; }
	public int? S3S4CycleID { get; set; }
	public int? S3S4CycleStationID { get; set; }
	public DateTimeOffset? S3S4CycleTS { get; set; }

	public DTOS3S4Cycle? S3S4Cycle { get; set; }

	public DTOS1S2Cycle S1S2Cycle { get; set; }
}