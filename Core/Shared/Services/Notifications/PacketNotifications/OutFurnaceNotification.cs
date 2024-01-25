using Core.Entities.Packets.Models.DB.Furnaces.OutFurnaces;
using Core.Entities.Packets.Models.Structs;
using Core.Shared.Dictionaries;
using Microsoft.Extensions.Logging;

namespace Core.Shared.Services.Notifications.PacketNotifications;

/// <summary>
/// Creates the <see cref="BaseNotification{T,TStruct}"/> for the <see cref="OutFurnace"/> packet
/// </summary>
public class OutFurnaceNotification : PacketNotification<OutFurnaceStruct>
{
	public static async Task<OutFurnaceNotification> Create(dynamic ads, ILogger logger)
	{
		return (await CreateSub<OutFurnaceNotification>(
			ads,
			ADSUtils.OutFurnaceRemove,
			ADSUtils.OutFurnaceNewMsg,
			ADSUtils.OutFurnaceAcquitMsg,
			ADSUtils.OutFurnaceToRead,
			logger) as OutFurnaceNotification)!;
	}
}