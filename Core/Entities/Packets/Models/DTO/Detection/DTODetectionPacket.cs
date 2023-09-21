using Core.Entities.Packets.Models.DB;
using Core.Entities.Packets.Models.DB.Detection;
using Core.Shared.Models.DTOs.Kernel.Interfaces;

namespace Core.Entities.Packets.Models.DTO.Detection;

public class DTODetectionPacket : DTOPacket, IDTO<DetectionPacket, DTODetectionPacket>
{
	public DTODetectionPacket(Packet packet) : base(packet)
	{
	}
}