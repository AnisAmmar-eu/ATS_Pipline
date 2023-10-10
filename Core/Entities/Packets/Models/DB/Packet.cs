using Core.Entities.Packets.Models.DTO;
using Core.Shared.Models.DB.Kernel;
using Core.Shared.Models.DB.Kernel.Interfaces;

namespace Core.Entities.Packets.Models.DB;

public partial class Packet : BaseEntity, IBaseEntity<Packet, DTOPacket>
{
	public string StationCycleRID { get; set; }
	public string Status { get; set; }
	public string Type { get; set; }
	public bool HasError { get; set; } = false;
}