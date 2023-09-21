using Core.Entities.AlarmsRT.Models.DB;
using Core.Entities.AlarmsRT.Models.DTO;
using Core.Entities.AlarmsRT.Services;
using Core.Entities.Packets.Models.DB;
using Core.Entities.Packets.Models.DTO;
using Core.Migrations;
using Core.Shared.UnitOfWork.Interfaces;

namespace Core.Entities.Packets.Services;

public class PacketService : IPacketService
{
	private readonly IAlarmUOW _alarmUOW;

	public PacketService(IAlarmUOW alarmUOW)
	{
		_alarmUOW = alarmUOW;
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
}