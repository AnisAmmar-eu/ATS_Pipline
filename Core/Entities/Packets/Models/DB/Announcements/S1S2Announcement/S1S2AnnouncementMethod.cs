using Core.Entities.Packets.Models.DTO.Announcements.S1S2Announcements;
using Core.Entities.Packets.Models.Structs;
using Core.Shared.Models.DB.Kernel.Interfaces;

namespace Core.Entities.Packets.Models.DB.Announcements.S1S2Announcement;

public partial class S1S2Announcement : Announcement, IBaseEntity<S1S2Announcement, DTOS1S2Announcement>
{
	public S1S2Announcement()
	{
	}

	public S1S2Announcement(AnnouncementStruct adsStruct) : base(adsStruct)
	{
		SerialNumber = adsStruct.SerialNumber;
		TrolleyNumber = (int)adsStruct.TrolleyNumber;
	}

	public override DTOS1S2Announcement ToDTO()
	{
		return new DTOS1S2Announcement(this);
	}
}