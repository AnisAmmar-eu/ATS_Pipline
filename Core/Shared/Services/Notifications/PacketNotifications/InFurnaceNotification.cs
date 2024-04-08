using Core.Entities.Packets.Models.DB.Furnaces.InFurnaces;
using Core.Entities.Packets.Models.Structs;
using Microsoft.Extensions.Logging;

namespace Core.Shared.Services.Notifications.PacketNotifications;

/// <summary>
/// Creates the <see cref="BaseNotification{T,TStruct}"/> for the <see cref="InFurnace"/> packet
/// </summary>
public class InFurnaceNotification : PacketNotification<InFurnaceStruct>
{
	public InFurnaceNotification(
		uint newMsg,
		uint oldEntry,
		ILogger logger) : base(newMsg, oldEntry, logger)
	{
	}
}