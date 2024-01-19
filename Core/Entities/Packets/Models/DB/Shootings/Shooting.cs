using Core.Entities.Packets.Models.DTO.Shootings;
using Core.Shared.Models.DB.Kernel.Interfaces;

namespace Core.Entities.Packets.Models.DB.Shootings;

public partial class Shooting : Packet, IBaseEntity<Shooting, DTOShooting>
{
	public string AnodeType { get; set; } = string.Empty;
	public int AnodeIDKey { get; set; }
	public string GlobalStationStatus { get; set; } = string.Empty; // TODO Dictionary ?
	public string LedStatus { get; set; } = string.Empty; // TODO Dictionary ?
	public int ProcedurePerformance { get; set; } // TODO int ?
	public DateTimeOffset ShootingTS { get; set; }
	private string ImagePath { get; } = string.Empty;
	private string ThumbnailPath { get; } = string.Empty;
}