using Core.Entities.Packets.Dictionaries;
using Core.Entities.Packets.Models.DTO.Announcements;
using Core.Entities.Packets.Models.Structs;
using Core.Entities.StationCycles.Models.DB;
using Core.Entities.StationCycles.Models.DB.MatchableCycles.S3S4Cycles;
using Core.Shared.Dictionaries;
using Core.Shared.UnitOfWork.Interfaces;

namespace Core.Entities.Packets.Models.DB.Announcements;

public partial class Announcement
{
	public Announcement()
	{
	}

	public Announcement(DTOAnnouncement dtoAnnouncement) : base(dtoAnnouncement)
	{
		StationCycleRID = dtoAnnouncement.StationCycleRID;
		AnnounceID = dtoAnnouncement.AnnounceID;
		AnodeType = dtoAnnouncement.AnodeType;
	}

	public Announcement(AnnouncementStruct adsStruct)
	{
		StationCycleRID = adsStruct.CycleRID.ToRID();
		Status = (PacketStatus)adsStruct.Status;
		TS = adsStruct.TS.GetTimestamp();
		AnodeType = adsStruct.AnodeType switch {
			1 => AnodeTypeDict.DX,
			2 => AnodeTypeDict.D20,
			_ => AnodeTypeDict.Undefined,
		};
		// TODO
		//AnnounceID = adsStruct.AnnounceID.ToRID();
	}

	public override DTOAnnouncement ToDTO()
	{
		return new(this);
	}
}