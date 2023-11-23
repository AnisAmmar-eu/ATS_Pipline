using Core.Entities.Packets.Models.DB;
using Core.Shared.Models.DTO.Kernel;
using Core.Shared.Models.DTO.Kernel.Interfaces;

namespace Core.Entities.Packets.Models.DTO;

public partial class DTOPacket : DTOBaseEntity, IDTO<Packet, DTOPacket>, IExtensionBinder<DTOPacket>
{
	public string StationCycleRID { get; set; }
	public string Status { get; set; }
	public string Type { get; set; }
	public bool HasError { get; set; }
}