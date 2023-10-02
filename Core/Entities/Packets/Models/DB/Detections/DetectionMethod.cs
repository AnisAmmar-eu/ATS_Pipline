using Core.Entities.Packets.Dictionary;
using Core.Entities.Packets.Models.DTO.Detections;
using Core.Entities.Packets.Models.Structs;
using Core.Shared.Models.DB.Kernel.Interfaces;

namespace Core.Entities.Packets.Models.DB.Detections;

public partial class Detection : Packet, IBaseEntity<Detection, DTODetection>
{
	public Detection()
	{
	}

	public Detection(DetectionStruct detectionStruct)
	{
		Type = PacketType.DETECTION;
		//CycleStationRID = detectionStruct.CycleStationRID; // TODO will be a struct
		MeasuredType = (AnodeType)detectionStruct.MeasuredType;
		IsMismatched = detectionStruct.IsMismatched;
		AnodeSize = detectionStruct.AnodeSize;
	}

	public override DTODetection ToDTO(string? languageRID = null)
	{
		return new DTODetection(this);
	}
}