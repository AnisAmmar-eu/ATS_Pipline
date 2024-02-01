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
	public OutFurnaceNotification(
		uint newMsg,
		uint oldEntry,
		ILogger logger) : base(newMsg, oldEntry, logger)
	{
	}
}