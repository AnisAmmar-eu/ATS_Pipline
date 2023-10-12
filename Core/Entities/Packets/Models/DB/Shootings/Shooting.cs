using Core.Entities.Packets.Models.DTO.Shootings;
using Core.Shared.Models.DB.Kernel.Interfaces;

namespace Core.Entities.Packets.Models.DB.Shootings;

public partial class Shooting : Packet, IBaseEntity<Shooting, DTOShooting>
{
	public int AnodeIDKey { get; set; } // IESA key
	public string GlobalStationStatus { get; set; } // TODO Dictionary ?
	public string LedStatus { get; set; } // TODO Dictionary ?
	public int ProcedurePerformance { get; set; } // TODO int ?
	public DateTimeOffset TSShooting { get; set; } // TSWhenAnodeIsShot
}