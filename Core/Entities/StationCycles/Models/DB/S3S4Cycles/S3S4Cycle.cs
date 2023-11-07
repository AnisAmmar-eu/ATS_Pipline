using Core.Entities.Packets.Models.DB.Furnaces.InFurnaces;
using Core.Entities.Packets.Models.DB.Furnaces.OutFurnaces;
using Core.Entities.StationCycles.Dictionaries;
using Core.Entities.StationCycles.Interfaces;
using Core.Entities.StationCycles.Models.DTO.S3S4Cycles;
using Core.Shared.Models.DB.Kernel.Interfaces;

namespace Core.Entities.StationCycles.Models.DB.S3S4Cycles;

public partial class S3S4Cycle : StationCycle, IBaseEntity<S3S4Cycle, DTOS3S4Cycle>, IMatchableCycle
{
	public string? AnnounceID { get; set; }
	public SignMatchStatus MatchingCamera1 { get; set; } = SignMatchStatus.NA;
	public SignMatchStatus MatchingCamera2 { get; set; } = SignMatchStatus.NA;

	public string? InFurnaceStatus { get; set; }
	public int? InFurnaceID { get; set; }
	public InFurnace? InFurnacePacket { get; set; }

	public string? OutFurnaceStatus { get; set; }
	public int? OutFurnaceID { get; set; }
	public OutFurnace? OutFurnacePacket { get; set; }
}