using Core.Entities.Packets.Models.DTO.Furnaces;
using Core.Shared.Models.DB.Kernel.Interfaces;

namespace Core.Entities.Packets.Models.DB.Furnaces;

public abstract partial class Furnace : Packet, IBaseEntity<Furnace, DTOFurnace>;