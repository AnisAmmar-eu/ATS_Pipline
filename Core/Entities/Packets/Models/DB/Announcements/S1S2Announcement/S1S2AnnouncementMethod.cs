using Core.Entities.Packets.Models.DTO.Announcements.S1S2Announcements;
using Core.Entities.Packets.Models.Structs;

namespace Core.Entities.Packets.Models.DB.Announcements.S1S2Announcement;

public partial class S1S2Announcement
{
	public S1S2Announcement()
	{
	}

	public S1S2Announcement(DTOS1S2Announcement dtos1S2Announcement) : base(dtos1S2Announcement)
	{
		TrolleyNumber = dtos1S2Announcement.TrolleyNumber;
		SerialNumber = dtos1S2Announcement.SerialNumber;
	}

	public S1S2Announcement(AnnouncementStruct adsStruct) : base(adsStruct)
	{
		SerialNumber = adsStruct.SerialNumber;
		TrolleyNumber = adsStruct.TrolleyNumber;
	}

	public override DTOS1S2Announcement ToDTO()
	{
		return new(this);
	}
}