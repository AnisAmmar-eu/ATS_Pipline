using Core.Entities.Packets.Models.DB.Shootings;
using Core.Shared.Models.DTOs.Kernel.Interfaces;

namespace Core.Entities.Packets.Models.DTO.Shootings;

public partial class DTOShooting : DTOPacket, IDTO<Shooting, DTOShooting>
{
	public int AnodeIDKey; // IESA key
	public string GlobalStationStatus; // TODO Dictionary ?
	public string LedStatus; // TODO Dictionary ?
	public int ProcedurePerformance; // TODO int ?
	public DateTimeOffset ShootingTS; // TSWhenAnodeIsShot
}