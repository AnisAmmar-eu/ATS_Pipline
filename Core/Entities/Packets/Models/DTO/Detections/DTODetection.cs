using Core.Entities.Packets.Models.DB.Detections;
using Core.Shared.Models.DTO.Kernel.Interfaces;

namespace Core.Entities.Packets.Models.DTO.Detections;

public partial class DTODetection : DTOPacket, IDTO<Detection, DTODetection>
{
	public int AnodeSize; // Millimeters
	public bool IsMismatched;
	public int MeasuredType; // Enum AnodeType
}