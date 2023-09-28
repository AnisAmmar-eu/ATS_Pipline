using Core.Entities.Packets.Models.DB.Furnaces.InFurnaces;
using Core.Shared.Models.DTOs.Kernel.Interfaces;

namespace Core.Entities.Packets.Models.DTO.Furnaces.InFurnaces;

public partial class DTOInFurnace : DTOFurnace, IDTO<InFurnace, DTOInFurnace>
{
	public int FurnaceNumber;
	public int PITNumber; // TODO PIT => Pit ?
	public int PITSectionNumber;
	
	public int FTAIDUsedToBake; // TODO ????
	public DateTimeOffset TSPackingPit; // TsPackingInPit
	public int AnodePosition; // AnodePositionInThePackOf7
	public int PITHeight; // HeightInThePit
}