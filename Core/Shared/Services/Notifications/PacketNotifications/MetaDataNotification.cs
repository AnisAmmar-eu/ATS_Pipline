using Core.Entities.Packets.Models.DB.MetaDatas;
using Core.Entities.Packets.Models.Structs;
using Core.Shared.Dictionaries;
using Microsoft.Extensions.Logging;

namespace Core.Shared.Services.Notifications.PacketNotifications;

/// <summary>
/// Creates the <see cref="BaseNotification{T,TStruct}"/> for the <see cref="MetaData"/> packet
/// </summary>
public class MetaDataNotification : PacketNotification<MetaDataStruct>
{
	public MetaDataNotification(
		uint newMsg,
		uint oldEntry,
		ILogger logger) : base(newMsg, oldEntry, logger)
	{
	}
}