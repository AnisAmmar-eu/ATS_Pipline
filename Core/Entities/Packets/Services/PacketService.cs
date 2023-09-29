using System.Text;
using Core.Entities.Packets.Dictionary;
using Core.Entities.Packets.Models.DB;
using Core.Entities.Packets.Models.DTO;
using Core.Migrations;
using Core.Shared.UnitOfWork.Interfaces;
using Newtonsoft.Json;

namespace Core.Entities.Packets.Services;

public class PacketService : IPacketService
{
	private readonly IAlarmUOW _alarmUOW;

	public PacketService(IAlarmUOW alarmUOW)
	{
		_alarmUOW = alarmUOW;
	}

	public async Task<DTOPacket> AddPacket(Packet packet)
	{
		await _alarmUOW.StartTransaction();
		await _alarmUOW.Packet.Add(packet);
		_alarmUOW.Commit();
		await _alarmUOW.CommitTransaction();
		return packet.ToDTO();
	}
	public async Task<DTOPacket> BuildPacket(DTOPacket dtoPacket)
	{
		await _alarmUOW.StartTransaction();
		
		Packet packet = dtoPacket.ToModel();
		await packet.Create(_alarmUOW);

		await packet.Build(_alarmUOW, packet.ToDTO());

		await _alarmUOW.CommitTransaction();
		return packet.ToDTO();
	}

	public async Task<HttpResponseMessage> SendPacketsToServer()
	{
		List<Packet> packets = await _alarmUOW.Packet.GetAll();
		const string api2Url = "https://localhost:7207/api/receive/packet";
		string jsonData = JsonConvert.SerializeObject(packets.ConvertAll(packet => packet.ToDTO()));
		StringContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");

		using (HttpClient httpClient = new HttpClient())
		{
			HttpResponseMessage response = await httpClient.PostAsync(api2Url, content);
			
			if (!response.IsSuccessStatusCode) return response;
			
			await _alarmUOW.StartTransaction();
			packets.ForEach(packet =>
			{
				_alarmUOW.Packet.Remove(packet);
			});
			_alarmUOW.Commit();
			await _alarmUOW.CommitTransaction();

			return response;
		}
	}

	public async Task ReceivePacket(IEnumerable<DTOPacket> packets)
	{
		await _alarmUOW.StartTransaction();
		foreach (DTOPacket packet in packets)
		{
			packet.ID = 0;
			await _alarmUOW.Packet.Add(packet.ToModel());
		}
		_alarmUOW.Commit();
		await _alarmUOW.CommitTransaction();
	}
}