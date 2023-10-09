using System.Net.NetworkInformation;
using Core.Entities.ServicesMonitors.Models.DB;
using Core.Entities.ServicesMonitors.Models.DTO;
using Core.Entities.ServicesMonitors.Repositories;
using Core.Shared.Services.Kernel;
using Core.Shared.UnitOfWork.Interfaces;

namespace Core.Entities.ServicesMonitors.Services;

public class ServicesMonitorService :
	ServiceBaseEntity<IServicesMonitorRepository, ServicesMonitor, DTOServicesMonitor>,
	IServicesMonitorService
{
	public ServicesMonitorService(IAnodeUOW anodeUOW) : base(anodeUOW)
	{
	}

	public async Task PingAllAndUpdate()
	{
		List<ServicesMonitor> services = await AnodeUOW.ServicesMonitor.GetAll();
		List<Tuple<Task<PingReply>, ServicesMonitor>> replies = new();
		foreach (ServicesMonitor service in services)
		{
			if (service.IPAddress == "")
				continue;
			Ping ping = new();
			replies.Add(new Tuple<Task<PingReply>, ServicesMonitor>
				(ping.SendPingAsync(service.IPAddress, 3000), service));
		}

		foreach ((Task<PingReply>? taskPing, ServicesMonitor? service) in replies)
		{
			PingReply reply = await taskPing;
			service.IsConnected = reply.Status == IPStatus.Success;
			await Update(service);
		}
	}
}