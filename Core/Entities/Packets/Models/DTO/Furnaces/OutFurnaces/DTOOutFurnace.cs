using Core.Entities.Packets.Models.DB.Furnaces.OutFurnaces;
using Core.Shared.Models.DTO.Kernel.Interfaces;

namespace Core.Entities.Packets.Models.DTO.Furnaces.OutFurnaces;

public partial class DTOOutFurnace : DTOFurnace, IDTO<OutFurnace, DTOOutFurnace>
{
	public string OutAnnounceID { get; set; } = string.Empty;
	public int FTAPickUp { get; set; }
	public DateTimeOffset? PickUpTS { get; set; }
	public DateTimeOffset? DepositTS { get; set; }
	public int InvalidPacket { get; set; }
}