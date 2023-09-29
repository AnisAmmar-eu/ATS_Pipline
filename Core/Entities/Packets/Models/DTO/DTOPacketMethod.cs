using Core.Entities.Packets.Models.DB;
using Core.Shared.Models.DTOs.Kernel;
using Core.Shared.Models.DTOs.Kernel.Interfaces;

namespace Core.Entities.Packets.Models.DTO;

public partial class DTOPacket : DTOBaseEntity, IDTO<Packet, DTOPacket>
{
	public DTOPacket(Packet detection)
	{
		ID = detection.ID;
		TS = detection.TS;
		CycleStationRID = detection.CycleStationRID;
		Status = detection.Status;
		Type = detection.Type;
	}

	public DTOPacket()
	{
		CycleStationRID = "";
		Type = "";
	}

	public override Packet ToModel()
	{
		return new Packet(this);
	}
}