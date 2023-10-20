using Core.Entities.Packets.Models.DTO.Detections;
using Core.Shared.Models.DB.Kernel.Interfaces;

namespace Core.Entities.Packets.Models.DB.Detections;

public partial class Detection : Packet, IBaseEntity<Detection, DTODetection>
{
	public int AnodeSize { get; set; } // LaserAnodeSize (millimeters)
	public bool IsSameType { get; set; } // AnodeTypeSizeMismatch
	public string MeasuredType { get; set; } = string.Empty;
}