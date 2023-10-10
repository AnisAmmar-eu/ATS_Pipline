using Core.Entities.Packets.Models.DB.Announcements;
using Core.Entities.StationCycles.Models.DTO.S1S2Cycles;
using Core.Shared.Models.DB.Kernel.Interfaces;

namespace Core.Entities.StationCycles.Models.DB.S1S2Cycles;

public partial class S1S2Cycle : StationCycle, IBaseEntity<S1S2Cycle, DTOS1S2Cycle>
{
	public string AnnouncementStatus2 { get; set; }
	public int AnnouncementID2 { get; set; }
	
	#region Nav Properties
	
	private Announcement? _announcement2;

	public virtual Announcement AnnouncementPacket2
	{
		set => _announcement2 = value;
		get => _announcement2
		       ?? throw new InvalidOperationException("Uninitialized property: " + nameof(Announcement));
	}
	
	#endregion
}