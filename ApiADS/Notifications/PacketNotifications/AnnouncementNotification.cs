using Core.Entities.Packets.Models.Structs;
using Core.Shared.Dictionaries;

namespace ApiADS.Notifications.PacketNotifications;

public class AnnouncementNotification : PacketNotification<AnnouncementStruct>
{
	public static async Task<AnnouncementNotification> Create(dynamic ads)
	{
		return (await CreateSub<AnnouncementNotification>(ads, ADSUtils.AnnouncementRemove,
			ADSUtils.AnnouncementNewMsg, ADSUtils.AnnouncementToRead) as AnnouncementNotification)!;
	}
}