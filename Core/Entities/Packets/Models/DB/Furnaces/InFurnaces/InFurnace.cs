using Core.Entities.Packets.Models.DTO.Furnaces.InFurnaces;
using Core.Shared.Models.DB.Kernel.Interfaces;

namespace Core.Entities.Packets.Models.DB.Furnaces.InFurnaces;

public partial class InFurnace : Furnace, IBaseEntity<InFurnace, DTOInFurnace>
{
	public string InAnnounceID { get; set; } = string.Empty;
	public int OriginID { get; set; }
	public int PackPosition { get; set; }
	public int PalletSide { get; set; }
	public int PitNumber { get; set; }
	public int PitSectionNumber { get; set; }
	public int PitHeight { get; set; }
	public int FTAPlace { get; set; }
	public int GreenConvPos { get; set; }
	public DateTimeOffset PitLoadTS { get; set; }
}