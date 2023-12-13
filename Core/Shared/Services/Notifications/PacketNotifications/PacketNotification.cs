using Core.Entities.Packets.Models.DB;
using Core.Entities.Packets.Models.DB.AlarmLists;
using Core.Entities.Packets.Models.DB.Shootings;
using Core.Entities.Packets.Services;
using Core.Shared.Models.DB.Kernel.Interfaces;
using Core.Shared.SignalR.StationCycleHub;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Shared.Services.Notifications.PacketNotifications;

public class PacketNotification<TStruct> : BaseNotification<Packet, TStruct>
	where TStruct : struct, IBaseADS<Packet>
{
	protected override async Task AddElement(IServiceProvider services, Packet entity)
	{
		IPacketService packetService = services.GetRequiredService<IPacketService>();
		IHubContext<StationCycleHub, IStationCycleHub> hubContext
			= services.GetRequiredService<IHubContext<StationCycleHub, IStationCycleHub>>();
		await packetService.BuildPacket(entity);
		if (entity is Shooting shooting)
		{
			Packet alarmList = new AlarmList {
				StationCycleRID = shooting.StationCycleRID,
				StationCycle = shooting.StationCycle,
			};
			await packetService.BuildPacket(alarmList);
		}

		await hubContext.Clients.All.RefreshStationCycle();
	}
}