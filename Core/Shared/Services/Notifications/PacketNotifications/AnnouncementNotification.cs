using Core.Entities.Packets.Models.DB.Announcements;
using Core.Entities.Packets.Models.Structs;
using Core.Shared.Dictionaries;
using Microsoft.Extensions.Logging;

namespace Core.Shared.Services.Notifications.PacketNotifications;

/// <summary>
/// Creates the <see cref="BaseNotification{T,TStruct}"/> for the <see cref="Announcement"/> packet
/// </summary>
public class AnnouncementNotification : PacketNotification<AnnouncementStruct>
{
	public static async Task<AnnouncementNotification> Create(dynamic ads, ILogger logger)
	{
		return (await CreateSub<AnnouncementNotification>(
			ads,
			ADSUtils.AnnouncementRemove,
			ADSUtils.AnnouncementNewMsg,
			ADSUtils.AnnouncementAcquitMsg,
			ADSUtils.AnnouncementToRead,
			logger) as AnnouncementNotification)!;
	}
}