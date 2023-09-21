using Core.Entities.Packets.Models.DTO;
using Core.Entities.Packets.Models.DTO.Detection;
using Core.Shared.Models.DB.Kernel.Interfaces;

namespace Core.Entities.Packets.Models.DB.Detection;

public partial class DetectionPacket : Packet, IBaseEntity<DetectionPacket, DTODetectionPacket>
{
	// TODO
	public int Placeholder { get; set; }

	public DetectionPacket(DTOPacket dtoPacket) : base(dtoPacket)
	{
	}
}