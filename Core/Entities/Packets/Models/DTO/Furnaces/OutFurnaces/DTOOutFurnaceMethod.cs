using Core.Entities.Packets.Dictionaries;
using Core.Entities.Packets.Models.DB.Furnaces.OutFurnaces;

namespace Core.Entities.Packets.Models.DTO.Furnaces.OutFurnaces;

public partial class DTOOutFurnace
{
	public DTOOutFurnace()
	{
		Type = PacketTypes.OutFurnace;
	}

	public DTOOutFurnace(OutFurnace outFurnace) : base(outFurnace)
	{
		Type = PacketTypes.OutFurnace;
		OutAnnounceID = outFurnace.OutAnnounceID;
		FTAPickUp = outFurnace.FTAPickUp;
		PickUpTS = outFurnace.PickUpTS;
		DepositTS = outFurnace.DepositTS;
		InvalidPacket = outFurnace.InvalidPacket;
		BakedConvPos = outFurnace.BakedConvPos;
	}

	public override OutFurnace ToModel()
	{
		return new(this);
	}
}