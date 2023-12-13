using Core.Entities.Packets.Dictionaries;
using Core.Entities.Packets.Models.DB.Detections;

namespace Core.Entities.Packets.Models.DTO.Detections;

public partial class DTODetection
{
	public DTODetection()
	{
		Type = PacketType.Detection;
	}

	public DTODetection(Detection packet) : base(packet)
	{
		Type = PacketType.Detection;
		MeasuredType = packet.MeasuredType;
		IsSameType = packet.IsSameType;
		AnodeSize = packet.AnodeSize;
	}

	public override Detection ToModel()
	{
		return new(this);
	}
}