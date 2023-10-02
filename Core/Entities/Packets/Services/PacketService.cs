using System.Text;
using Core.Entities.Packets.Models.DB;
using Core.Entities.Packets.Models.DTO;
using Core.Entities.Packets.Repositories;
using Core.Shared.Services.Kernel;
using Core.Shared.UnitOfWork.Interfaces;
using Newtonsoft.Json;

namespace Core.Entities.Packets.Services;

public class PacketService : ServiceBaseEntity<IPacketRepository, Packet, DTOPacket>, IPacketService
{
	public PacketService(IAlarmUOW alarmUOW) : base(alarmUOW)
	{
	}

	public async Task<DTOPacket> BuildPacket(DTOPacket dtoPacket)
	{
		await AlarmUOW.StartTransaction();

		Packet packet = dtoPacket.ToModel();
		await packet.Create(AlarmUOW);

		await packet.Build(AlarmUOW, packet.ToDTO());

		await AlarmUOW.CommitTransaction();
		return packet.ToDTO();
	}

	public async Task<HttpResponseMessage> SendPacketsToServer()
	{
		List<Packet> packets = await AlarmUOW.Packet.GetAll();
		const string api2Url = "https://localhost:7207/api/receive/packet";
		string jsonData = JsonConvert.SerializeObject(packets.ConvertAll(packet => packet.ToDTO()));
		StringContent content = new(jsonData, Encoding.UTF8, "application/json");

		using (HttpClient httpClient = new())
		{
			HttpResponseMessage response = await httpClient.PostAsync(api2Url, content);

			if (!response.IsSuccessStatusCode) return response;

			await AlarmUOW.StartTransaction();
			packets.ForEach(packet => { AlarmUOW.Packet.Remove(packet); });
			AlarmUOW.Commit();
			await AlarmUOW.CommitTransaction();

			return response;
		}
	}

	public async Task ReceivePacket(IEnumerable<DTOPacket> packets)
	{
		await AlarmUOW.StartTransaction();
		foreach (DTOPacket packet in packets)
		{
			packet.ID = 0;
			await AlarmUOW.Packet.Add(packet.ToModel());
		}

		AlarmUOW.Commit();
		await AlarmUOW.CommitTransaction();
	}
}