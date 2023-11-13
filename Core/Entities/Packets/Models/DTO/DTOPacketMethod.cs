using Core.Entities.Packets.Models.DB;

namespace Core.Entities.Packets.Models.DTO;

public partial class DTOPacket
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