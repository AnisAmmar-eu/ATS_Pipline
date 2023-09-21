using Core.Entities.AlarmsCycle.Models.DB;
using Core.Entities.AlarmsRT.Models.DB;
using Core.Entities.Packets.Models.DTO;
using Core.Shared.Models.DB.Kernel.Interfaces;
using Core.Shared.UnitOfWork.Interfaces;
using DTOAlarmListPacket = Core.Entities.Packets.Models.DTO.AlarmListPackets.DTOAlarmListPacket;

namespace Core.Entities.Packets.Models.DB.AlarmListPackets;

public partial class AlarmListPacket : Packet, IBaseEntity<AlarmListPackets.AlarmListPacket, DTOAlarmListPacket>
{

	public AlarmListPacket() : base()
	{
		AlarmList = new List<AlarmCycle>();
	}
	public AlarmListPacket(DTOAlarmListPacket dtoPacket) : base(dtoPacket)
	{
		AlarmList = dtoPacket.AlarmList.ToList().ConvertAll(dtoAlarmCycle =>
		{
			AlarmCycle alarmCycle = dtoAlarmCycle.ToModel();
			alarmCycle.AlarmListPacket = this;
			return alarmCycle;
		});
	}
	public override DTOAlarmListPacket ToDTO(string? languageRID = null)
	{
		return new DTOAlarmListPacket(this);
	}

	protected override async Task<DTOPacket> InheritedBuild(IAlarmUOW alarmUOW, DTOPacket dtoPacket)
	{
		DTOAlarmListPacket dtoAlarmListPacket = (DTOAlarmListPacket)dtoPacket;
		List<AlarmRT> alarmRTs = await alarmUOW.AlarmRT.GetAllWithInclude();
		foreach (AlarmRT alarmRT in alarmRTs)
		{
			AlarmCycle alarmCycle = new AlarmCycle(alarmRT);
			alarmCycle.AlarmListPacketID = dtoAlarmListPacket.ID;
			await alarmUOW.AlarmCycle.Add(alarmCycle);
			alarmUOW.Commit();	
			dtoAlarmListPacket.AlarmList.Add(alarmCycle.ToDTO());
		}

		return dtoAlarmListPacket;
	}
}