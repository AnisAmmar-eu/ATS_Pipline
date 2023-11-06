using Core.Entities.Anodes.Models.DB;
using Core.Entities.Packets.Dictionaries;
using Core.Entities.StationCycles.Models.DTO.S1S2Cycles;
using Core.Entities.StationCycles.Models.DTO.S3S4Cycles;
using Core.Shared.Models.DTO.Kernel;
using Core.Shared.Models.DTO.Kernel.Interfaces;

namespace Core.Entities.Anodes.Models.DTO;

public partial class DTOAnode: DTOBaseEntity, IDTO<Anode, DTOAnode>
{
	public string S1S2CycleRID { get; set; } = string.Empty;
	public string Status { get; set; } = PacketStatus.Initialized;
	public DateTimeOffset? ClosedTS { get; set; }
	
	public int S1S2CycleID { get; set; }
	public int? S3S4CycleID { get; set; }
	
	public DTOS3S4Cycle? S3S4Cycle { get; set; }
	
	public DTOS1S2Cycle S1S2Cycle { get; set; }
}