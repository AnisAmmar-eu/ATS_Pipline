using System.Text.Json.Serialization;
using Core.Entities.Packets.Dictionaries;
using Core.Entities.Packets.Models.DB;
using Core.Entities.Packets.Models.DTO.AlarmLists;
using Core.Entities.Packets.Models.DTO.Announcements;
using Core.Entities.Packets.Models.DTO.Furnaces;
using Core.Entities.Packets.Models.DTO.Shootings;
using Core.Shared.Models.DTO.Kernel;
using Core.Shared.Models.DTO.Kernel.Interfaces;

namespace Core.Entities.Packets.Models.DTO;

[JsonDerivedType(typeof(DTOAlarmList))]
[JsonDerivedType(typeof(DTOAnnouncement))]
[JsonDerivedType(typeof(DTOFurnace))]
[JsonDerivedType(typeof(DTOShooting))]
public partial class DTOPacket : DTOBaseEntity, IDTO<Packet, DTOPacket>
{
	public string StationCycleRID { get; set; } = string.Empty;
	public PacketStatus Status { get; set; } = PacketStatus.Initialized;
	public string Type { get; set; } = string.Empty;
	public bool HasError { get; set; }
}