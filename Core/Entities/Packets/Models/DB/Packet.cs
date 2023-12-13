using Core.Entities.Packets.Dictionaries;
using Core.Entities.Packets.Models.DTO;
using Core.Entities.StationCycles.Models.DB;
using Core.Shared.Models.DB.Kernel;
using Core.Shared.Models.DB.Kernel.Interfaces;

namespace Core.Entities.Packets.Models.DB;

public partial class Packet : BaseEntity, IBaseEntity<Packet, DTOPacket>
{
	public string StationCycleRID { get; set; } = string.Empty;
	public PacketStatus Status { get; set; } = PacketStatus.Initialized;
	public bool HasError { get; set; }
	public StationCycle? StationCycle { get; set; }
}