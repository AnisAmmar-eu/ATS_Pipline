using Core.Entities.Packets.Dictionaries;
using Core.Entities.Packets.Models.DB.Furnaces.OutFurnaces;

namespace Core.Entities.Packets.Models.DTO.Furnaces.OutFurnaces;

public partial class DTOOutFurnace
{
	public DTOOutFurnace()
	{
		Type = PacketType.OutFurnace;
	}

	public DTOOutFurnace(OutFurnace outFurnace) : base(outFurnace)
	{
		Type = PacketType.OutFurnace;
		OutAnnounceID = outFurnace.OutAnnounceID;
		FTAPickUp = outFurnace.FTAPickUp;
		PickUpTS = outFurnace.PickUpTS;
		DepositTS = outFurnace.DepositTS;
		InvalidPacket = outFurnace.InvalidPacket;
	}

	public override OutFurnace ToModel()
	{
		return new(this);
	}
}