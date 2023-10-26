using System.Text;
using Core.Entities.Packets.Dictionaries;
using Core.Entities.Packets.Models.DB;
using Core.Entities.Packets.Models.DTO;
using Core.Entities.Packets.Repositories;
using Core.Shared.Services.Kernel;
using Core.Shared.UnitOfWork.Interfaces;
using Newtonsoft.Json;

namespace Core.Entities.Packets.Services;

public class PacketService : ServiceBaseEntity<IPacketRepository, Packet, DTOPacket>, IPacketService
{
	public PacketService(IAnodeUOW anodeUOW) : base(anodeUOW)
	{
	}

	public async Task<DTOPacket> BuildPacket(Packet packet)
	{
		await AnodeUOW.StartTransaction();

		await packet.Create(AnodeUOW);

		await packet.Build(AnodeUOW);

		await AnodeUOW.CommitTransaction();
		return packet.ToDTO();
	}

	public async Task<HttpResponseMessage> SendPacketsToServer()
	{
		List<Packet> packets = await AnodeUOW.Packet.GetAll();
		const string api2Url = "https://localhost:7207/apiServerReceive/packets";
		string jsonData = JsonConvert.SerializeObject(packets.ConvertAll(packet => packet.ToDTO()));
		StringContent content = new(jsonData, Encoding.UTF8, "application/json");

		using (HttpClient httpClient = new())
		{
			HttpResponseMessage response = await httpClient.PostAsync(api2Url, content);

			if (!response.IsSuccessStatusCode || !packets.Any()) return response;

			await AnodeUOW.StartTransaction();
			packets.ForEach(packet => { AnodeUOW.Packet.Remove(packet); });
			AnodeUOW.Commit();
			await AnodeUOW.CommitTransaction();

			return response;
		}
	}

	public async Task<List<Packet>> ReceivePackets(IEnumerable<DTOPacket> dtoPackets)
	{
		await AnodeUOW.StartTransaction();
		List<Packet> packets = new();
		foreach (DTOPacket dtoPacket in dtoPackets)
		{
			Packet packet = dtoPacket.ToModel();
			packet.ID = 0;
			// DBContext operations should NOT be done concurrently hence why await in loop.
			await AnodeUOW.Packet.Add(packet);
			packets.Add(packet);
		}

		if (!packets.Any()) return packets;

		AnodeUOW.Commit();
		await AnodeUOW.CommitTransaction();
		return packets;
	}

	public async Task<int?> AddPacketFromStationCycle(Packet? packet)
	{
		if (packet == null)
			return null;
		packet.ID = 0;
		await AnodeUOW.Packet.Add(packet);
		AnodeUOW.Commit();
		return packet.ID;
	}

	public void MarkPacketAsSentFromStationCycle(Packet? packet)
	{
		if (packet == null)
			return;
		packet.Status = PacketStatus.Sent;
		AnodeUOW.Packet.Update(packet);
	}
}