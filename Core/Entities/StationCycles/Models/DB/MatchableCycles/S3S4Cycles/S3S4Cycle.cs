using Core.Entities.Packets.Models.DB.Furnaces.InFurnaces;
using Core.Entities.Packets.Models.DB.Furnaces.OutFurnaces;
using Core.Entities.StationCycles.Models.DB.MatchableCycles.S5Cycles;
using Core.Entities.StationCycles.Models.DTO.MatchingCycles.S3S4Cycles;
using Core.Shared.Models.DB.Kernel.Interfaces;

namespace Core.Entities.StationCycles.Models.DB.MatchableCycles.S3S4Cycles;

public partial class S3S4Cycle : MatchableCycle, IBaseEntity<S3S4Cycle, DTOS3S4Cycle>
{
	public string? AnnounceID { get; set; }

	public int? InFurnaceID { get; set; }
	public InFurnace? InFurnacePacket { get; set; }

	public int? OutFurnaceID { get; set; }
	public OutFurnace? OutFurnacePacket { get; set; }

	public S5Cycle? ChainCycle { get; set; }
}