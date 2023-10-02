using Core.Entities.Packets.Models.DTO.Furnaces.InFurnaces;
using Core.Entities.Packets.Models.Structs;
using Core.Shared.Models.DB.Kernel.Interfaces;

namespace Core.Entities.Packets.Models.DB.Furnaces.InFurnaces;

public partial class InFurnace : Furnace, IBaseEntity<InFurnace, DTOInFurnace>
{
	public InFurnace(InFurnaceStruct adsStruct)
	{
		// TODO
		//CycleStationRID = adsStruct.CycleStationRID;
		OriginID = (int)adsStruct.OriginID;
		AnodePosition = (int)adsStruct.AnodePosition;
		PalletSide = (int)adsStruct.PalletSide;
		PITNumber = (int)adsStruct.PITNumber;
		PITSectionNumber = (int)adsStruct.PITSectionNumber;
		PITHeight = (int)adsStruct.PITHeight;
		FTAinPIT = (int)adsStruct.FTAinPIT;
		GreenPosition = (int)adsStruct.GreenPosition;
		BakedPosition = (int)adsStruct.BakedPosition;
		FTASuckPit = (int)adsStruct.FTASuckPit;
		// TODO
		// TSLoad = adsStruct.TSLoad;
	}

	public override DTOInFurnace ToDTO(string? languageRID = null)
	{
		return new DTOInFurnace(this);
	}
}