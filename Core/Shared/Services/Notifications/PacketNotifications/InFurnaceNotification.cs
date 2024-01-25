using Core.Entities.Packets.Models.Structs;
using Core.Shared.Dictionaries;
using Microsoft.Extensions.Logging;

namespace Core.Shared.Services.Notifications.PacketNotifications;

public class InFurnaceNotification : PacketNotification<InFurnaceStruct>
{
	public static async Task<InFurnaceNotification> Create(dynamic ads, ILogger logger)
	{
		return (await CreateSub<InFurnaceNotification>(
			ads,
			ADSUtils.InFurnaceRemove,
			ADSUtils.InFurnaceNewMsg,
			ADSUtils.InFurnaceAcquitMsg,
			ADSUtils.InFurnaceToRead,
			logger) as InFurnaceNotification)!;
	}
}