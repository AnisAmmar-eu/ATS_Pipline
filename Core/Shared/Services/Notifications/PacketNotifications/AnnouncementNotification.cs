using Core.Entities.Packets.Models.Structs;
using Core.Shared.Dictionaries;
using Microsoft.Extensions.Logging;

namespace Core.Shared.Services.Notifications.PacketNotifications;

public class AnnouncementNotification : PacketNotification<AnnouncementStruct>
{
	public static async Task<AnnouncementNotification> Create(dynamic ads, ILogger logger)
	{
		return (await CreateSub<AnnouncementNotification>(
			ads,
			ADSUtils.AnnouncementRemove,
			ADSUtils.AnnouncementNewMsg,
			ADSUtils.AnnouncementToRead,
			logger) as AnnouncementNotification)!;
	}
}