using Core.Entities.Packets.Dictionaries;
using Core.Entities.Packets.Models.DB.Detections;

namespace Core.Entities.Packets.Models.DTO.Detections;

public partial class DTODetection
{
	public DTODetection()
	{
		Type = PacketType.Detection;
	}

	public DTODetection(Detection detection) : base(detection)
	{
		Type = PacketType.Detection;
		MeasuredType = detection.MeasuredType;
		IsSameType = detection.IsSameType;
		AnodeSize = detection.AnodeSize;
	}

	public override Detection ToModel()
	{
		return new(this);
	}
}