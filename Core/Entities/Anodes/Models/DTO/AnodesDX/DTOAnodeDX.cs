using Core.Entities.Anodes.Dictionaries;
using Core.Entities.Anodes.Models.DB.AnodesDX;
using Core.Entities.StationCycles.Dictionaries;
using Core.Entities.StationCycles.Models.DTO.MatchingCycles.S5Cycles;
using Core.Shared.Models.DTO.Kernel.Interfaces;

namespace Core.Entities.Anodes.Models.DTO.AnodesDX;

public partial class DTOAnodeDX : DTOAnode, IDTO<AnodeDX, DTOAnodeDX>
{
	new public string AnodeType { get; set; } = AnodeTypes.DX;

	public int? S5CycleID { get; set; }
	public DTOS5Cycle? S5Cycle { get; set; }
	public string? S5StationID { get; set; }
	public DateTimeOffset? S5TSFirstShooting { get; set; }
	public SignMatchStatus SSignStatus1 { get; set; } = SignMatchStatus.NA;
	public SignMatchStatus S5SignStatus2 { get; set; } = SignMatchStatus.NA;
	public SignMatchStatus S5MatchingCamera1 { get; set; } = SignMatchStatus.NA;
	public SignMatchStatus S5MatchingCamera2 { get; set; } = SignMatchStatus.NA;
}