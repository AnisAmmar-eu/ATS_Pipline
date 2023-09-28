using Core.Entities.Packets.Models.DB.Furnaces;
using Core.Entities.Packets.Models.DB.Furnaces.InFurnaces;
using Core.Shared.Models.DTOs.Kernel.Interfaces;

namespace Core.Entities.Packets.Models.DTO.Furnaces.InFurnaces;

public partial class DTOInFurnace : DTOFurnace, IDTO<InFurnace, DTOInFurnace>
{
	public DTOInFurnace(InFurnace furnace) : base(furnace)
	{
		FurnaceNumber = furnace.FurnaceNumber;
		PITNumber = furnace.PITNumber;
		PITSectionNumber = furnace.PITSectionNumber;
		FTAIDUsedToBake = furnace.FTAIDUsedToBake;
		TSPackingPit = furnace.TSPackingPit;
		AnodePosition = furnace.AnodePosition;
		PITHeight = furnace.PITHeight;
	}
}