using Core.Entities.Packets.Models.DB;
using Core.Shared.Models.DTO.Kernel;
using Core.Shared.Models.DTO.Kernel.Interfaces;

namespace Core.Entities.Packets.Models.DTO;

public partial class DTOPacket : DTOBaseEntity, IDTO<Packet, DTOPacket>
{
	public DTOPacket(Packet detection)
	{
		ID = detection.ID;
		TS = detection.TS;
		CycleStationRID = detection.StationCycleRID;
		Status = detection.Status;
		Type = detection.Type;
		HasError = detection.HasError;
	}

	public DTOPacket()
	{
		CycleStationRID = "";
		Type = "";
		Status = "";
	}

	public override Packet ToModel()
	{
		return new Packet(this);
	}
}