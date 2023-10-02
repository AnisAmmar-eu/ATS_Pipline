using Core.Entities.Packets.Models.DB.Furnaces.InFurnaces;
using Core.Shared.Models.DTO.Kernel.Interfaces;

namespace Core.Entities.Packets.Models.DTO.Furnaces.InFurnaces;

public partial class DTOInFurnace : DTOFurnace, IDTO<InFurnace, DTOInFurnace>
{
	public DTOInFurnace(InFurnace inFurnace) : base(inFurnace)
	{
		OriginID = inFurnace.OriginID;
		AnodePosition = inFurnace.AnodePosition;
		PalletSide = inFurnace.PalletSide;
		PITNumber = inFurnace.PITNumber;
		PITSectionNumber = inFurnace.PITSectionNumber;
		PITHeight = inFurnace.PITHeight;
		FTAinPIT = inFurnace.FTAinPIT;
		GreenPosition = inFurnace.GreenPosition;
		BakedPosition = inFurnace.BakedPosition;
		FTASuckPit = inFurnace.FTASuckPit;
		TSLoad = inFurnace.TSLoad;
	}
}