using Core.Entities.Packets.Models.DB.Furnaces.InFurnaces;
using Core.Entities.Packets.Models.Structs;
using Core.Shared.Dictionaries;
using Microsoft.Extensions.Logging;

namespace Core.Shared.Services.Notifications.PacketNotifications;

/// <summary>
/// Creates the <see cref="BaseNotification{T,TStruct}"/> for the <see cref="InFurnace"/> packet
/// </summary>
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