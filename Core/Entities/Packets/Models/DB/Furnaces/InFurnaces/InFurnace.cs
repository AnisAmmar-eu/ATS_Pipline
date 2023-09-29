using Core.Entities.Packets.Models.DTO.Furnaces.InFurnaces;
using Core.Shared.Models.DB.Kernel.Interfaces;

namespace Core.Entities.Packets.Models.DB.Furnaces.InFurnaces;

public partial class InFurnace : Furnace, IBaseEntity<InFurnace, DTOInFurnace>
{
	public int OriginID;
	public int AnodePosition; // AnodePositionInThePackOf7
	public int PalletSide;
	public int PITNumber;
	public int PITSectionNumber;
	public int PITHeight;
	public int FTAinPIT;
	public int GreenPosition;
	public int BakedPosition;
	public int FTASuckPit;
	public DateTimeOffset? TSLoad;
}