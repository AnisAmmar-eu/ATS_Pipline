using Core.Entities.Packets.Dictionary;
using Core.Entities.Packets.Models.DTO;
using Core.Entities.Packets.Models.DTO.Detections;
using Core.Shared.Models.DB.Kernel.Interfaces;

namespace Core.Entities.Packets.Models.DB.Detections;

public partial class Detection : Packet, IBaseEntity<Detection, DTODetection>
{
	public int AnodeSize; // LaserAnodeSize (millimeters)
	public bool IsMismatched; // AnodeTypeSizeMismatch
	public AnodeType MeasuredType;

	public Detection(DTOPacket dtoPacket) : base(dtoPacket)
	{
	}
}