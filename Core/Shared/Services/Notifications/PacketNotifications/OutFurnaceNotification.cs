using Core.Entities.Packets.Models.Structs;
using Core.Shared.Dictionaries;
using Microsoft.Extensions.Logging;

namespace Core.Shared.Services.Notifications.PacketNotifications;

public class OutFurnaceNotification : PacketNotification<OutFurnaceStruct>
{
	public static async Task<OutFurnaceNotification> Create(dynamic ads, ILogger logger)
	{
		return (await CreateSub<OutFurnaceNotification>(
			ads,
			ADSUtils.OutFurnaceRemove,
			ADSUtils.OutFurnaceNewMsg,
			ADSUtils.OutFurnaceToRead,
			logger) as OutFurnaceNotification)!;
	}
}