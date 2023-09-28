using Core.Entities.Packets.Models.DB;
using Core.Entities.Packets.Models.DB.Detections;
using Core.Shared.Models.DTOs.Kernel.Interfaces;

namespace Core.Entities.Packets.Models.DTO.Detections;

public partial class DTODetection : DTOPacket, IDTO<Detection, DTODetection>
{
	public DTODetection(Detection detection) : base(detection)
	{
		MeasuredType = (int)detection.MeasuredType;
		IsMismatched = detection.IsMismatched;
		AnodeSize = detection.AnodeSize;
	}
}