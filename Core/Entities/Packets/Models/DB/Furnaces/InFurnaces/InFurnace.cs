using Core.Entities.Packets.Models.DTO.Furnaces.InFurnaces;
using Core.Shared.Models.DB.Kernel.Interfaces;

namespace Core.Entities.Packets.Models.DB.Furnaces.InFurnaces;

public partial class InFurnace : Furnace, IBaseEntity<InFurnace, DTOInFurnace>
{
	public int FurnaceNumber;
	public int PITNumber; // TODO PIT => Pit ?
	public int PITSectionNumber;
	
	public int FTAIDUsedToBake; // TODO ????
	public DateTimeOffset TSPackingPit; // TsPackingInPit
	public int AnodePosition; // AnodePositionInThePackOf7
	public int PITHeight; // HeightInThePit
}