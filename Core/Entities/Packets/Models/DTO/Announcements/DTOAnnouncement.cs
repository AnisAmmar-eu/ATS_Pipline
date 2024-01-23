using System.Text.Json.Serialization;
using Core.Entities.Packets.Models.DB.Announcements;
using Core.Entities.Packets.Models.DTO.Announcements.S1S2Announcements;
using Core.Shared.Models.DTO.Kernel.Interfaces;

namespace Core.Entities.Packets.Models.DTO.Announcements;

[JsonDerivedType(typeof(DTOS1S2Announcement))]
public partial class DTOAnnouncement : DTOPacket, IDTO<Announcement, DTOAnnouncement>
{
	public string AnodeType { get; set; } = string.Empty;
	public int SyncIndex { get; set; }
	public bool IsDouble { get; set; }
}