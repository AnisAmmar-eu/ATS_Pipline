using Core.Entities.Packets.Models.DB;
using Core.Entities.Packets.Models.DTO.Binders;
using Core.Entities.StationCycles.Models.DB;
using Core.Shared.Models.DTO.Kernel;
using Core.Shared.Models.DTO.Kernel.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Core.Entities.Packets.Models.DTO;

[ModelBinder(typeof(DTOPacketBinder))]
public partial class DTOPacket : DTOBaseEntity, IDTO<Packet, DTOPacket>
{
	public string StationCycleRID { get; set; }
	public string Status { get; set; }
	public string Type { get; set; }
	public bool HasError { get; set; } = false;
	
	public StationCycle? StationCycle { get; set; }
}