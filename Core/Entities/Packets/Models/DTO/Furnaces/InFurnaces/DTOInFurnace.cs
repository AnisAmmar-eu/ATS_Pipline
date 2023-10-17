using Core.Entities.Packets.Models.DB.Furnaces.InFurnaces;
using Core.Shared.Models.DTO.Kernel.Interfaces;

namespace Core.Entities.Packets.Models.DTO.Furnaces.InFurnaces;

public partial class DTOInFurnace : DTOFurnace, IDTO<InFurnace, DTOInFurnace>
{
	public int AnnounceID { get; set; }
	public int OriginID { get; set; }
	public int PackPosition { get; set; }
	public int PalletSide { get; set; }
	public int PitNumber { get; set; }
	public int PitSectionNumber { get; set; }
	public int PitHeight { get; set; }
	public int FTAPlace { get; set; }
	public int FTASuck { get; set; }
	public int GreenConvPos { get; set; }
	public int BakedConvPos { get; set; }
	public DateTimeOffset PitLoadTS { get; set; }
}