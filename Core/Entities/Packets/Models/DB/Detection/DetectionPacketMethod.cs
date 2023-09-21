using Core.Entities.Packets.Models.DTO.Detection;
using Core.Shared.Models.DB.Kernel.Interfaces;

namespace Core.Entities.Packets.Models.DB.Detection;

public partial class DetectionPacket : Packet, IBaseEntity<DetectionPacket, DTODetectionPacket>
{
	public DetectionPacket() : base()
	{
		// TODO
	}
	public override DTODetectionPacket ToDTO(string? languageRID = null)
	{
		throw new NotImplementedException();
	}
}