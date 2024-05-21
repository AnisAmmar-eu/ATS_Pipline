using Core.Entities.Anodes.Models.DTO;
using Core.Entities.StationCycles.Dictionaries;
using Core.Entities.StationCycles.Models.DB.LoadableCycles.S1S2Cycles;
using Core.Entities.StationCycles.Models.DB.MatchableCycles.S3S4Cycles;
using Core.Shared.Models.DB.Kernel;
using Core.Shared.Models.DB.Kernel.Interfaces;

namespace Core.Entities.Anodes.Models.DB;

public abstract partial class Anode : BaseEntity, IBaseEntity<Anode, DTOAnode>
{
	public string CycleRID { get; set; } = string.Empty;
	public string SerialNumber { get; set; } = "NA";

	public int? S1S2CycleID { get; set; }
	public S1S2Cycle? S1S2Cycle { get; set; }
	public DateTimeOffset? S1S2TSFirstShooting { get; set; }
	public SignMatchStatus S1S2SignStatus1 { get; set; } = SignMatchStatus.NA;
	public SignMatchStatus S1S2SignStatus2 { get; set; } = SignMatchStatus.NA;

	public int? S3S4CycleID { get; set; }
	public S3S4Cycle? S3S4Cycle { get; set; }
	public DateTimeOffset? S3S4TSFirstShooting { get; set; }
	public SignMatchStatus S3S4SignStatus1 { get; set; } = SignMatchStatus.NA;
	public SignMatchStatus S3S4SignStatus2 { get; set; } = SignMatchStatus.NA;
	public SignMatchStatus SS3S4MatchingCamera1 { get; set; } = SignMatchStatus.NA;
	public SignMatchStatus S3S4MatchingCamera2 { get; set; } = SignMatchStatus.NA;
}