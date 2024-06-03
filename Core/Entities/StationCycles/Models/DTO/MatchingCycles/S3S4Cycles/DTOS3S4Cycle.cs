using Core.Entities.Packets.Models.DTO.Furnaces.InFurnaces;
using Core.Entities.Packets.Models.DTO.Furnaces.OutFurnaces;
using Core.Entities.StationCycles.Dictionaries;
using Core.Entities.StationCycles.Models.DB.MatchableCycles.S3S4Cycles;
using Core.Shared.Models.DTO.Kernel.Interfaces;

namespace Core.Entities.StationCycles.Models.DTO.MatchingCycles.S3S4Cycles;

public partial class DTOS3S4Cycle : DTOMatchingCycle, IDTO<S3S4Cycle, DTOS3S4Cycle>
{
	new public string CycleType { get; set; } = CycleTypes.S3S4;
	public string? AnnounceID { get; set; }

	public int? InFurnaceID { get; set; }
	public DTOInFurnace? InFurnacePacket { get; set; }

	public int? OutFurnaceID { get; set; }
	public DTOOutFurnace? OutFurnacePacket { get; set; }
}