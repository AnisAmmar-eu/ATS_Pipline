using System.Text.Json.Serialization;
using Core.Entities.Anodes.Dictionaries;
using Core.Entities.Anodes.Models.DB;
using Core.Entities.Anodes.Models.DTO.AnodesD20;
using Core.Entities.Anodes.Models.DTO.AnodesDX;
using Core.Entities.StationCycles.Dictionaries;
using Core.Entities.StationCycles.Models.DTO.LoadableCycles.S1S2Cycles;
using Core.Entities.StationCycles.Models.DTO.MatchingCycles.S3S4Cycles;
using Core.Shared.Models.DTO.Kernel;
using Core.Shared.Models.DTO.Kernel.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Core.Entities.Anodes.Models.DTO;

[JsonDerivedType(typeof(DTOAnodeD20))]
[JsonDerivedType(typeof(DTOAnodeDX))]
public partial class DTOAnode :
	DTOBaseEntity,
	IDTO<Anode, DTOAnode>,
	IBindableFromHttpContext<DTOAnode>
{
	public string CycleRID { get; set; } = string.Empty;
	public string AnodeType { get; set; } = AnodeTypes.UNDEFINED;
	public string SerialNumber { get; set; } = "NA";

	public int? S1S2CycleID { get; set; }
	public DTOS1S2Cycle? S1S2Cycle { get; set; }
	public DateTimeOffset? S1S2TSFirstShooting { get; set; }
	public SignMatchStatus S1S2SignStatus1 { get; set; } = SignMatchStatus.NA;
	public SignMatchStatus S1S2SignStatus2 { get; set; } = SignMatchStatus.NA;

	public int? S3S4CycleID { get; set; }
	public DTOS3S4Cycle? S3S4Cycle { get; set; }
	public DateTimeOffset? S3S4TSFirstShooting { get; set; }
	public SignMatchStatus S3S4SignStatus1 { get; set; } = SignMatchStatus.NA;
	public SignMatchStatus S3S4SignStatus2 { get; set; } = SignMatchStatus.NA;
	public SignMatchStatus SS3S4MatchingCamera1 { get; set; } = SignMatchStatus.NA;
	public SignMatchStatus S3S4MatchingCamera2 { get; set; } = SignMatchStatus.NA;

	public bool IsComplete { get; set; }
}