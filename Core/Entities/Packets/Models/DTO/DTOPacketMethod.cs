using Core.Entities.Packets.Models.DB;
using Core.Shared.Models.DTO.Kernel;
using Core.Shared.Models.DTO.Kernel.Interfaces;

namespace Core.Entities.Packets.Models.DTO;

public partial class DTOPacket : DTOBaseEntity, IDTO<Packet, DTOPacket>
{
	public DTOPacket()
	{
		StationCycleRID = "";
		Type = "";
		Status = "";
	}

	public DTOPacket(Packet detection)
	{
		Type = "";
		ID = detection.ID;
		TS = detection.TS;
		StationCycleRID = detection.StationCycleRID;
		Status = detection.Status;
		HasError = detection.HasError;
	}

	public override Packet ToModel()
	{
		return new Packet(this);
	}
}