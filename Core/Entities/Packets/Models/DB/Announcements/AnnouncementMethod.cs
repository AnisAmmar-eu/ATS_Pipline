using Core.Entities.Packets.Models.DTO.Announcements;
using Core.Entities.Packets.Models.Structs;
using Core.Shared.Dictionaries;

namespace Core.Entities.Packets.Models.DB.Announcements;

public partial class Announcement
{
	public Announcement()
	{
	}

	public Announcement(DTOAnnouncement dtoAnnouncement) : base(dtoAnnouncement)
	{
		StationCycleRID = dtoAnnouncement.StationCycleRID;
		AnodeType = dtoAnnouncement.AnodeType;
		SyncIndex =dtoAnnouncement.SyncIndex;
		IsDouble = dtoAnnouncement.IsDouble;
	}

	public Announcement(AnnouncementStruct adsStruct)
	{
		StationCycleRID = adsStruct.CycleRID.ToRID();
		TwinCatStatus = adsStruct.Status;
		TS = adsStruct.TS.GetTimestamp();
		SyncIndex = adsStruct.SyncIndex;
		IsDouble = adsStruct.Double == 1;
		AnodeType = AnodeTypeDict.AnodeTypeIntToString(adsStruct.AnodeType);
	}

	public override DTOAnnouncement ToDTO()
	{
		return new(this);
	}
}