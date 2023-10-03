using Core.Entities.Packets.Models.DTO.Furnaces.InFurnaces;
using Core.Shared.Models.DB.Kernel.Interfaces;

namespace Core.Entities.Packets.Models.DB.Furnaces.InFurnaces;

public partial class InFurnace : Furnace, IBaseEntity<InFurnace, DTOInFurnace>
{
	public int AnodePosition { get; set; } // AnodePositionInThePackOf7
	public int BakedPosition { get; set; }
	public int FTAinPIT { get; set; }
	public int FTASuckPit { get; set; }
	public int GreenPosition { get; set; }
	public int OriginID { get; set; }
	public int PalletSide { get; set; }
	public int PITHeight { get; set; }
	public int PITNumber { get; set; }
	public int PITSectionNumber { get; set; }
	public DateTimeOffset? TSLoad { get; set; }
}