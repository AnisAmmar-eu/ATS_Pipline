using System.Text.Json.Serialization;
using Core.Entities.Anodes.Dictionaries;
using Core.Entities.Anodes.Models.DB;
using Core.Entities.Anodes.Models.DTO.AnodesD20;
using Core.Entities.Anodes.Models.DTO.AnodesDX;
using Core.Entities.Packets.Dictionaries;
using Core.Entities.StationCycles.Models.DTO.LoadableCycles.S1S2Cycles;
using Core.Entities.StationCycles.Models.DTO.MatchingCycles.S3S4Cycles;
using Core.Shared.Models.DTO.Kernel;
using Core.Shared.Models.DTO.Kernel.Interfaces;

namespace Core.Entities.Anodes.Models.DTO;

[JsonDerivedType(typeof(DTOAnodeD20))]
[JsonDerivedType(typeof(DTOAnodeDX))]
public partial class DTOAnode : DTOBaseEntity, IDTO<Anode, DTOAnode>,
	IExtensionBinder<DTOAnode>
{
	public string CycleRID { get; set; } = string.Empty;
	public string AnodeType { get; set; } = AnodeTypes.UNDEFINED;

	public int? S1S2CycleID { get; set; }
	public DTOS1S2Cycle? S1S2Cycle { get; set; }

	public int? S3S4CycleID { get; set; }
	public DTOS3S4Cycle? S3S4Cycle { get; set; }
}