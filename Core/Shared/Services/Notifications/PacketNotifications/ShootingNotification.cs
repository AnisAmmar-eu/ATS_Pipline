using Core.Entities.Packets.Models.DB.Shootings;
using Core.Entities.Packets.Models.Structs;
using Core.Shared.Dictionaries;
using Microsoft.Extensions.Logging;

namespace Core.Shared.Services.Notifications.PacketNotifications;

/// <summary>
/// Creates the <see cref="BaseNotification{T,TStruct}"/> for the <see cref="Shooting"/> packet
/// </summary>
public class ShootingNotification : PacketNotification<ShootingStruct>
{
	public static async Task<ShootingNotification?> Create(dynamic ads, ILogger logger)
	{
		return await CreateSub<ShootingNotification>(
			ads,
			ADSUtils.ShootingRemove,
			ADSUtils.ShootingNewMsg,
			ADSUtils.ShootingAcquitMsg,
			ADSUtils.ShootingToRead,
			logger) as ShootingNotification;
	}
}