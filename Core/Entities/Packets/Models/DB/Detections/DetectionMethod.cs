using Core.Entities.Packets.Dictionaries;
using Core.Entities.Packets.Models.DTO.Detections;
using Core.Entities.Packets.Models.Structs;
using Core.Shared.Dictionaries;
using Core.Shared.Models.DB.Kernel.Interfaces;

namespace Core.Entities.Packets.Models.DB.Detections;

public partial class Detection : Packet, IBaseEntity<Detection, DTODetection>
{
	public Detection()
	{
	}

	public Detection(DetectionStruct detectionStruct)
	{
		Type = PacketType.Detection;
		//CycleStationRID = detectionStruct.CycleStationRID; // TODO will be a struct
		MeasuredType = (AnodeType)detectionStruct.MeasuredType;
		IsMismatched = detectionStruct.IsMismatched;
		AnodeSize = detectionStruct.AnodeSize;
	}

	public override DTODetection ToDTO()
	{
		return new DTODetection(this);
	}
}