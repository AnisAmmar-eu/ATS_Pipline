using Core.Entities.Packets.Models.DB.Announcements.S1S2Announcement;
using Core.Entities.StationCycles.Models.DTO.S1S2Cycles;

namespace Core.Entities.StationCycles.Models.DB.S1S2Cycles;

public partial class S1S2Cycle
{
	public S1S2Cycle()
	{
	}

	public S1S2Cycle(DTOS1S2Cycle dtoS1S2Cycle) : base(dtoS1S2Cycle)
	{
		// Query will set the announce in the parent attribute instead of the overriden one.
		if (dtoS1S2Cycle.AnnouncementPacket == null && base.AnnouncementPacket != null)
			AnnouncementPacket = (S1S2Announcement?)base.AnnouncementPacket;
		else AnnouncementPacket = dtoS1S2Cycle.AnnouncementPacket?.ToModel();
	}

	public override DTOS1S2Cycle ToDTO()
	{
		return new DTOS1S2Cycle(this);
	}
}