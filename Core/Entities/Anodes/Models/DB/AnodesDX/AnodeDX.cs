using Core.Entities.Anodes.Models.DTO.AnodesDX;
using Core.Entities.StationCycles.Dictionaries;
using Core.Entities.StationCycles.Models.DB.MatchableCycles.S5Cycles;
using Core.Shared.Models.DB.Kernel.Interfaces;

namespace Core.Entities.Anodes.Models.DB.AnodesDX;

public partial class AnodeDX : Anode, IBaseEntity<AnodeDX, DTOAnodeDX>
{
	public int? S5CycleID { get; set; }
	public int? S5StationID { get; set; }

	public S5Cycle? S5Cycle { get; set; }

	public DateTimeOffset? S5TSFirstShooting { get; set; }
	public SignMatchStatus SSignStatus1 { get; set; } = SignMatchStatus.NA;
	public SignMatchStatus S5SignStatus2 { get; set; } = SignMatchStatus.NA;
	public SignMatchStatus S5MatchingCamera1 { get; set; } = SignMatchStatus.NA;
	public SignMatchStatus S5MatchingCamera2 { get; set; } = SignMatchStatus.NA;
}