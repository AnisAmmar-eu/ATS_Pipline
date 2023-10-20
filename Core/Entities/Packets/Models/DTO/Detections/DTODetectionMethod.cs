using Core.Entities.Packets.Dictionaries;
using Core.Entities.Packets.Models.DB.Detections;
using Core.Shared.Models.DTO.Kernel.Interfaces;

namespace Core.Entities.Packets.Models.DTO.Detections;

public partial class DTODetection : DTOPacket, IDTO<Detection, DTODetection>
{
	public DTODetection(Detection detection) : base(detection)
	{
		Type = PacketType.Detection;
		MeasuredType = detection.MeasuredType;
		IsSameType = detection.IsSameType;
		AnodeSize = detection.AnodeSize;
	}

	public override Detection ToModel()
	{
		return new Detection(this);
	}
}