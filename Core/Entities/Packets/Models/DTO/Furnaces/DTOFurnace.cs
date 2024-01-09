using System.Text.Json.Serialization;
using Core.Entities.Packets.Models.DB.Furnaces;
using Core.Entities.Packets.Models.DTO.Furnaces.InFurnaces;
using Core.Entities.Packets.Models.DTO.Furnaces.OutFurnaces;
using Core.Shared.Models.DTO.Kernel.Interfaces;

namespace Core.Entities.Packets.Models.DTO.Furnaces;

[JsonDerivedType(typeof(DTOInFurnace))]
[JsonDerivedType(typeof(DTOOutFurnace))]
public partial class DTOFurnace : DTOPacket, IDTO<Furnace, DTOFurnace>;