using System.Text.Json.Serialization;
using Core.Entities.Packets.Models.DB.Shootings;
using Core.Entities.Packets.Models.DTO.Shootings.S3S4Shootings;
using Core.Entities.Packets.Models.DTO.Shootings.S5Shootings;
using Core.Shared.Models.DTO.Kernel.Interfaces;

namespace Core.Entities.Packets.Models.DTO.Shootings;

[JsonDerivedType(typeof(DTOS3S4Shooting))]
[JsonDerivedType(typeof(DTOS5Shooting))]
public partial class DTOShooting : DTOPacket, IDTO<Shooting, DTOShooting>
{
	public DateTimeOffset ShootingTS { get; set; }
	public string AnodeType { get; set; } = string.Empty;
	public int Cam01Status { get; set; }
	public int Cam02Status { get; set; }
}