using Core.Entities.Packets.Models.DB;
using Core.Entities.Packets.Models.DB.AlarmLists;
using Core.Entities.Packets.Models.DB.MetaDatas;
using Core.Entities.Packets.Models.DB.Shootings;
using Core.Entities.Packets.Services;
using Core.Shared.Configuration;
using Core.Shared.Dictionaries;
using Core.Shared.Models.DB.Kernel.Interfaces;
using Core.Shared.SignalR.StationCycleHub;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Core.Shared.Services.Notifications.PacketNotifications;

/// <summary>
/// Implements an override of the <see cref="AddElement"/> function for every packet queue.
/// </summary>
/// <typeparam name="TStruct"></typeparam>
public class PacketNotification<TStruct> : BaseNotification<Packet, TStruct>
	where TStruct : struct, IBaseADS<Packet>
{
	public PacketNotification(
		uint newMsg,
		uint oldEntry,
		ILogger logger) : base(newMsg, oldEntry, logger)
	{
	}

	/// <summary>
	/// Builds and add the <see cref="entity"/> packet in the table, will verify its images if it is a <see cref="Shooting"/> packet.
	/// </summary>
	/// <param name="services"></param>
	/// <param name="entity"></param>
	protected override async Task AddElement(IServiceProvider services, Packet entity)
	{
		IPacketService packetService = services.GetRequiredService<IPacketService>();
		IConfiguration configuration = services.GetRequiredService<IConfiguration>();
		IHubContext<StationCycleHub, IStationCycleHub> hubContext
			= services.GetRequiredService<IHubContext<StationCycleHub, IStationCycleHub>>();
		if (entity is MetaData metaData)
		{
			await packetService.BuildPacket(metaData);
			Packet alarmList = new AlarmList {
				StationCycleRID = metaData.StationCycleRID,
				StationCycle = metaData.StationCycle,
			};
			await packetService.BuildPacket(alarmList);
		}
		else
		{
			await packetService.BuildPacket(entity);
		}

		// SignalR
		await hubContext.Clients.All.RefreshStationCycle();
	}
}