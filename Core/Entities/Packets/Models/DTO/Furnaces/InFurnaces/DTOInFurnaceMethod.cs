using Core.Entities.Packets.Models.DB.Furnaces.InFurnaces;
using Core.Shared.Models.DTO.Kernel.Interfaces;

namespace Core.Entities.Packets.Models.DTO.Furnaces.InFurnaces;

public partial class DTOInFurnace : DTOFurnace, IDTO<InFurnace, DTOInFurnace>
{
	public DTOInFurnace(InFurnace inFurnace) : base(inFurnace)
	{
		AnnounceID = inFurnace.AnnounceID;
		OriginID = inFurnace.OriginID;
		PackPosition = inFurnace.PackPosition;
		PalletSide = inFurnace.PalletSide;
		PitNumber = inFurnace.PitNumber;
		PitSectionNumber = inFurnace.PitSectionNumber;
		PitHeight = inFurnace.PitHeight;
		FTAPlace = inFurnace.FTAPlace;
		FTASuck = inFurnace.FTASuck;
		GreenConvPos = inFurnace.GreenConvPos;
		BakedConvPos = inFurnace.BakedConvPos;
		PitLoadTS = inFurnace.PitLoadTS;
	}
}