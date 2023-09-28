using Core.Entities.Packets.Models.DB;
using Core.Entities.Packets.Models.DB.AlarmListPackets;
using Core.Shared.Models.DTOs.Kernel.Interfaces;
using Core.Entities.Alarms.AlarmsCycle.Models.DTO;
namespace Core.Entities.Packets.Models.DTO.AlarmListPackets;

public partial class DTOAlarmListPacket : DTOPacket, IDTO<AlarmListPacket, DTOAlarmListPacket>
{
	public DTOAlarmListPacket() : base()
	{
		AlarmList = new List<DTOAlarmCycle>();
	}
	public DTOAlarmListPacket(AlarmListPacket packet) : base(packet)
	{
		AlarmList = packet.AlarmList.ToList().ConvertAll(alarmCycle => alarmCycle.ToDTO());
	}
	
	public override AlarmListPacket ToModel()
	{
		return new AlarmListPacket(this);
	}
}