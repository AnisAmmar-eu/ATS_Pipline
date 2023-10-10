using Core.Entities.Packets.Dictionaries;
using Core.Entities.Packets.Models.DTO.Announcements;
using Core.Entities.Packets.Models.Structs;
using Core.Shared.Dictionaries;
using Core.Shared.Models.DB.Kernel.Interfaces;

namespace Core.Entities.Packets.Models.DB.Announcements;

public partial class Announcement : Packet, IBaseEntity<Announcement, DTOAnnouncement>
{
	public Announcement()
	{
	}

	public Announcement(AnnouncementStruct adsStruct)
	{
		// TODO
		// CycleStationRID = adsStruct.CycleStationRID;
		AnodeType = (AnodeType)adsStruct.AnodeType;
	}

	public override DTOAnnouncement ToDTO()
	{
		return new DTOAnnouncement(this);
	}
}