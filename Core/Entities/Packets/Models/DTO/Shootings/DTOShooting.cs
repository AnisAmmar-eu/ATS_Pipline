using System.Text.Json.Serialization;
using Core.Entities.Packets.Models.DB.Shootings;
using Core.Entities.Packets.Models.DTO.Shootings.S3S4Shootings;
using Core.Shared.Models.DTO.Kernel.Interfaces;

namespace Core.Entities.Packets.Models.DTO.Shootings;

[JsonDerivedType(typeof(DTOS3S4Shooting))]
public partial class DTOShooting : DTOPacket, IDTO<Shooting, DTOShooting>
{
	public string AnodeType { get; set; } = string.Empty;
	public int AnodeIDKey { get; set; }
	public string GlobalStationStatus { get; set; } = string.Empty; // TODO Dictionary ?
	public string LedStatus { get; set; } = string.Empty; // TODO Dictionary ?
	public int ProcedurePerformance { get; set; } // TODO int ?
	public DateTimeOffset ShootingTS { get; set; }
}