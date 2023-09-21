using Core.Entities.Packets.Dictionary;
using Core.Entities.Packets.Models.DB;
using Core.Shared.Models.DTOs.Kernel;
using Core.Shared.Models.DTOs.Kernel.Interfaces;

namespace Core.Entities.Packets.Models.DTO;

public partial class DTOPacket : DTOBaseEntity, IDTO<Packet, DTOPacket>
{
	public string CycleStationRID { get; set; }	
	public PacketStatus Status { get; set; }
	public string PacketType { get; set; }
	
}