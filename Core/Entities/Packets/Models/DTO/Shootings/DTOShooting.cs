using Core.Entities.Packets.Models.DB.Shootings;
using Core.Shared.Models.DTO.Kernel.Interfaces;

namespace Core.Entities.Packets.Models.DTO.Shootings;

public partial class DTOShooting : DTOPacket, IDTO<Shooting, DTOShooting>
{
	public DateTimeOffset ShootingTS { get; set; }
	public string AnodeType { get; set; } = string.Empty;
	public int Cam01Status { get; set; }
	public int Cam02Status { get; set; }
}