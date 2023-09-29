using Core.Entities.Packets.Dictionary;
using Core.Entities.Packets.Models.DB;
using Core.Entities.Packets.Models.DTO.Binders;
using Core.Shared.Models.DTOs.Kernel;
using Core.Shared.Models.DTOs.Kernel.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Core.Entities.Packets.Models.DTO;

[ModelBinder(typeof(DTOPacketBinder))]
public partial class DTOPacket : DTOBaseEntity, IDTO<Packet, DTOPacket>
{
	public string CycleStationRID { get; set; }	
	public PacketStatus Status { get; set; }
	public string Type { get; set; }
	
}