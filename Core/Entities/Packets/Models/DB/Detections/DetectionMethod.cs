using Core.Entities.Packets.Models.DTO.Detections;
using Core.Shared.Models.DB.Kernel.Interfaces;

namespace Core.Entities.Packets.Models.DB.Detections;

public partial class Detection : Packet, IBaseEntity<Detection, DTODetection>
{
	public Detection() : base()
	{
	}
	public override DTODetection ToDTO(string? languageRID = null)
	{
		return new DTODetection(this);
	}
}