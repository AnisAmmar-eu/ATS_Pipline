using Core.Entities.Packets.Dictionaries;
using Core.Entities.Packets.Models.DTO.Detections;
using Core.Entities.Packets.Models.Structs;
using Core.Entities.StationCycles.Models.DB;
using Core.Shared.Dictionaries;
using Core.Shared.Exceptions;
using Core.Shared.UnitOfWork.Interfaces;

namespace Core.Entities.Packets.Models.DB.Detections;

public partial class Detection
{
	public Detection()
	{
	}

	public Detection(DTODetection dtoDetection) : base(dtoDetection)
	{
		AnodeSize = dtoDetection.AnodeSize;
		IsSameType = dtoDetection.IsSameType;
		MeasuredType = dtoDetection.MeasuredType;
	}

	public Detection(DetectionStruct detectionStruct)
	{
		StationCycleRID = detectionStruct.StationCycleRID.ToRID();
		AnodeSize = detectionStruct.AnodeHigh;
		// Other fields are completed later.
	}

	public override DTODetection ToDTO()
	{
		return new(this);
	}
}