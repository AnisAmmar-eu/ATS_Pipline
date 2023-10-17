using Core.Entities.Packets.Models.Structs;
using TwinCAT.Ads;

namespace ApiADS.Notifications.PacketNotifications;

public class AnnouncementNotification : PacketNotification<AnnouncementStruct>
{
	public AnnouncementNotification(ResultHandle resultHandle, uint acquitMsg, uint newMsg, uint oldEntry)
		: base(resultHandle, acquitMsg, newMsg, oldEntry)
	{
	}

	public static async Task<AnnouncementNotification> Create(dynamic ads)
	{
		return (await CreateSub(ads, Utils.AnnouncementAcquitMsg, Utils.AnnouncementNewMsg, Utils.AnnouncementToRead) as AnnouncementNotification)!;
	}
}