using Core.Entities.Packets.Models.DTO.Furnaces.OutFurnaces;
using Core.Shared.Models.DB.Kernel.Interfaces;

namespace Core.Entities.Packets.Models.DB.Furnaces.OutFurnaces;

public partial class OutFurnace : Furnace, IBaseEntity<OutFurnace, DTOOutFurnace>
{
	public string AnnounceID { get; set; }
	public int FTAPickUp { get; set; }
	public DateTimeOffset? PickUpTS { get; set; }
	public DateTimeOffset? DepositTS { get; set; }
	public int InvalidPacket { get; set; }
}