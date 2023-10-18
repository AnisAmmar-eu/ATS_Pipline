using Core.Entities.Packets.Models.Structs;
using TwinCAT.Ads;

namespace ApiADS.Notifications.PacketNotifications;

public class AnnouncementNotification : PacketNotification<AnnouncementStruct>
{
	public AnnouncementNotification(ResultHandle resultHandle, uint remove, uint newMsg, uint oldEntry)
		: base(resultHandle, remove, newMsg, oldEntry)
	{
	}

	public static async Task<AnnouncementNotification> Create(dynamic ads)
	{
		return (await CreateSub<PacketNotification<AnnouncementStruct>>(ads, Utils.AnnouncementRemove,
			Utils.AnnouncementNewMsg, Utils.AnnouncementToRead) as AnnouncementNotification)!;
	}
}