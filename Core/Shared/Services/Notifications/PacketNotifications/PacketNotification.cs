using Core.Entities.Packets.Models.DB;
using Core.Entities.Packets.Models.DB.AlarmLists;
using Core.Entities.Packets.Models.DB.Shootings;
using Core.Entities.Packets.Services;
using Core.Shared.Configuration;
using Core.Shared.Models.DB.Kernel.Interfaces;
using Core.Shared.SignalR.StationCycleHub;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Shared.Services.Notifications.PacketNotifications;

public class PacketNotification<TStruct> : BaseNotification<Packet, TStruct>
	where TStruct : struct, IBaseADS<Packet>
{
	protected override async Task AddElement(IServiceProvider services, Packet entity)
	{
		IPacketService packetService = services.GetRequiredService<IPacketService>();
		IConfiguration configuration = services.GetRequiredService<IConfiguration>();
		IHubContext<StationCycleHub, IStationCycleHub> hubContext
			= services.GetRequiredService<IHubContext<StationCycleHub, IStationCycleHub>>();
		if (entity is Shooting shooting)
		{
			shooting.ImagesPath = configuration.GetValueWithThrow<string>("CameraConfig:ImagesPath");
			shooting.Extension = configuration.GetValueWithThrow<string>("CameraConfig:Extension");
			await packetService.BuildPacket(shooting);
			Packet alarmList = new AlarmList {
				StationCycleRID = shooting.StationCycleRID,
				StationCycle = shooting.StationCycle,
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