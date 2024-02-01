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
	public ShootingNotification(
		uint newMsg,
		uint oldEntry,
		ILogger logger) : base(newMsg, oldEntry, logger)
	{
	}
}