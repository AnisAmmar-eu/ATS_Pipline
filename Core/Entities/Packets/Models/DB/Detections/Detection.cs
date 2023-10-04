using Core.Entities.Packets.Dictionary;
using Core.Entities.Packets.Models.DTO;
using Core.Entities.Packets.Models.DTO.Detections;
using Core.Shared.Models.DB.Kernel.Interfaces;

namespace Core.Entities.Packets.Models.DB.Detections;

public partial class Detection : Packet, IBaseEntity<Detection, DTODetection>
{
	public Detection(DTOPacket dtoPacket) : base(dtoPacket)
	{
	}

	public int AnodeSize { get; set; } // LaserAnodeSize (millimeters)
	public bool IsMismatched { get; set; } // AnodeTypeSizeMismatch
	public AnodeType MeasuredType { get; set; }
}