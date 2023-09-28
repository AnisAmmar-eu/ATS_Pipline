using Core.Entities.Packets.Models.DTO.Shootings;
using Core.Shared.Models.DB.Kernel.Interfaces;

namespace Core.Entities.Packets.Models.DB.Shootings;

public partial class Shooting : Packet, IBaseEntity<Shooting, DTOShooting>
{
	public int AnodeIDKey; // IESA key
	public string GlobalStationStatus; // TODO Dictionary ?
	public int ProcedurePerformance; // TODO int ?
	public string LedStatus; // TODO Dictionary ?
	public DateTimeOffset ShootingTS; // TSWhenAnodeIsShot
}