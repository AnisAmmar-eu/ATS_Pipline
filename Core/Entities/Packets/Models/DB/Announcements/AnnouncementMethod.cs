using Core.Entities.Packets.Dictionaries;
using Core.Entities.Packets.Models.DTO.Announcements;
using Core.Entities.Packets.Models.Structs;
using Core.Entities.StationCycles.Models.DB;
using Core.Entities.StationCycles.Models.DB.S3S4Cycles;
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
		StationCycleRID = adsStruct.RID.ToRID();
		AnodeType = adsStruct.AnodeType switch
		{
			1 => AnodeTypeDict.DX,
			2 => AnodeTypeDict.D20,
			_ => AnodeTypeDict.Undefined
		};
		AnnounceID = adsStruct.AnnounceID.ToRID();
	}

	public override DTOAnnouncement ToDTO()
	{
		return new DTOAnnouncement(this);
	}

	protected override async Task InheritedBuild(IAnodeUOW anodeUOW)
	{
		// StationCycle does not exist yet. It needs to be created. We have a RID from ADS.
		StationCycle stationCycle = StationCycle.Create();
		stationCycle.RID = StationCycleRID;
		stationCycle.AnodeType = AnodeType;
		stationCycle.AnnouncementStatus = PacketStatus.Completed;
		stationCycle.AnnouncementID = ID;
		stationCycle.AnnouncementPacket = this;
		if (stationCycle is S3S4Cycle s3S4Cycle)
			s3S4Cycle.AnnounceID = AnnounceID;
		await anodeUOW.StationCycle.Add(stationCycle);
		Status = PacketStatus.Completed;
		StationCycle = stationCycle;
	}
}