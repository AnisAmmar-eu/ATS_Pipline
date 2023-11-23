using Core.Entities.Packets.Models.DB.Furnaces;
using Core.Shared.Models.DTO.Kernel.Interfaces;

namespace Core.Entities.Packets.Models.DTO.Furnaces;

public partial class DTOFurnace : DTOPacket, IDTO<Furnace, DTOFurnace>;