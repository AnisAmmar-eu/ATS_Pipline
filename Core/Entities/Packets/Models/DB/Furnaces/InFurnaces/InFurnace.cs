using Core.Entities.Packets.Models.DTO.Furnaces.InFurnaces;
using Core.Shared.Models.DB.Kernel.Interfaces;

namespace Core.Entities.Packets.Models.DB.Furnaces.InFurnaces;

public partial class InFurnace : Furnace, IBaseEntity<InFurnace, DTOInFurnace>
{
	public int AnodePosition; // AnodePositionInThePackOf7
	public int BakedPosition;
	public int FTAinPIT;
	public int FTASuckPit;
	public int GreenPosition;
	public int OriginID;
	public int PalletSide;
	public int PITHeight;
	public int PITNumber;
	public int PITSectionNumber;
	public DateTimeOffset? TSLoad;
}