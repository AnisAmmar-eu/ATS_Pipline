using Core.Entities.Packets.Models.DB;
using Core.Entities.Packets.Models.DB.Furnaces.OutFurnaces;
using Core.Shared.Models.DTO.Kernel.Interfaces;

namespace Core.Entities.Packets.Models.DTO.Furnaces.OutFurnaces;

public partial class DTOOutFurnace : DTOFurnace, IDTO<OutFurnace, DTOOutFurnace>
{
	public DTOOutFurnace() : base()
	{
		
	}
	public DTOOutFurnace(OutFurnace outFurnace) : base(outFurnace)
	{
		OutAnnounceID = outFurnace.OutAnnounceID;
		FTAPickUp = outFurnace.FTAPickUp;
		PickUpTS = outFurnace.PickUpTS;
		DepositTS = outFurnace.DepositTS;
		InvalidPacket = outFurnace.InvalidPacket;
	}

	public override OutFurnace ToModel()
	{
		return new OutFurnace(this);
	}
}