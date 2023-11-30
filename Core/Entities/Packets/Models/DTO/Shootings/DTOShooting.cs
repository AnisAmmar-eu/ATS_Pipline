using Core.Entities.Packets.Models.DB.Shootings;
using Core.Shared.Models.DTO.Kernel.Interfaces;

namespace Core.Entities.Packets.Models.DTO.Shootings;

public partial class DTOShooting : DTOPacket, IDTO<Shooting, DTOShooting>
{
	public int AnodeIDKey { get; set; }
	public string GlobalStationStatus { get; set; } = string.Empty; // TODO Dictionary ?
	public string LedStatus { get; set; } = string.Empty; // TODO Dictionary ?
	public int ProcedurePerformance { get; set; } // TODO int ?
	public DateTimeOffset ShootingTS { get; set; }
}