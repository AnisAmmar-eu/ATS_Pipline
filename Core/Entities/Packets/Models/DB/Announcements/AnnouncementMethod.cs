using Core.Entities.Packets.Dictionaries;
using Core.Entities.Packets.Models.DTO.Announcements;
using Core.Entities.Packets.Models.Structs;
using Core.Entities.StationCycles.Models.DB;
using Core.Shared.Dictionaries;
using Core.Shared.Models.DB.Kernel.Interfaces;
using Core.Shared.UnitOfWork.Interfaces;

namespace Core.Entities.Packets.Models.DB.Announcements;

public partial class Announcement : Packet, IBaseEntity<Announcement, DTOAnnouncement>
{
	public Announcement()
	{
		Type = PacketType.Announcement;
	}

	public Announcement(AnnouncementStruct adsStruct)
	{
		Type = PacketType.Announcement;
		StationCycleRID = adsStruct.RID.ToRID();
		AnodeType = adsStruct.AnodeType == 1 ? AnodeTypeDict.DX : AnodeTypeDict.D20;
	}

	public override DTOAnnouncement ToDTO()
	{
		return new DTOAnnouncement(this);
	}

	protected override async Task InheritedBuild(IAnodeUOW anodeUOW)
	{
		// StationCycle does not exist yet. It needs to be created. We have a RID from ADS.
		StationCycle stationCycle = new()
		{
			RID = StationCycleRID,
			AnodeType = AnodeType,
			AnnouncementStatus = PacketStatus.Completed,
			AnnouncementID = ID,
			AnnouncementPacket = this,
		};
		await anodeUOW.StationCycle.Add(stationCycle);
		Status = PacketStatus.Completed;
		StationCycle = stationCycle;
	}
}