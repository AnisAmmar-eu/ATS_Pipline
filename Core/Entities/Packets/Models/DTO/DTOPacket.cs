using Core.Entities.Packets.Dictionaries;
using Core.Entities.Packets.Models.DB;
using Core.Shared.Models.DTO.Kernel;
using Core.Shared.Models.DTO.Kernel.Interfaces;

namespace Core.Entities.Packets.Models.DTO;

public partial class DTOPacket : DTOBaseEntity, IDTO<Packet, DTOPacket>, IExtensionBinder<DTOPacket>
{
	public string StationCycleRID { get; set; } = string.Empty;
	public PacketStatus Status { get; set; } = PacketStatus.Initialized;
	public string Type { get; set; } = string.Empty;
	public bool HasError { get; set; }
}