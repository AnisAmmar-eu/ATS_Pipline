using Core.Entities.Alarms.AlarmsCycle.Models.DB;
using Core.Entities.Alarms.AlarmsRT.Models.DB;
using Core.Entities.Packets.Models.DTO;
using Core.Entities.Packets.Models.DTO.AlarmLists;
using Core.Shared.Models.DB.Kernel.Interfaces;
using Core.Shared.UnitOfWork.Interfaces;

namespace Core.Entities.Packets.Models.DB.AlarmLists;

public partial class AlarmList : Packet, IBaseEntity<AlarmList, DTOAlarmList>
{
	public AlarmList()
	{
		AlarmCycles = new List<AlarmCycle>();
	}

	public AlarmList(DTOAlarmList dto) : base(dto)
	{
		AlarmCycles = dto.AlarmList.ToList().ConvertAll(dtoAlarmCycle =>
		{
			AlarmCycle alarmCycle = dtoAlarmCycle.ToModel();
			alarmCycle.AlarmList = this;
			return alarmCycle;
		});
	}

	public override DTOAlarmList ToDTO(string? languageRID = null)
	{
		return new DTOAlarmList(this);
	}

	protected override async Task<DTOPacket> InheritedBuild(IAlarmUOW alarmUOW, DTOPacket dtoPacket)
	{
		DTOAlarmList dtoAlarmList = (DTOAlarmList)dtoPacket;
		List<AlarmRT> alarmRTs = await alarmUOW.AlarmRT.GetAllWithInclude();
		foreach (AlarmRT alarmRT in alarmRTs)
		{
			AlarmCycle alarmCycle = new(alarmRT);
			alarmCycle.AlarmListPacketID = dtoAlarmList.ID;
			await alarmUOW.AlarmCycle.Add(alarmCycle);
			alarmUOW.Commit();
			dtoAlarmList.AlarmList.Add(alarmCycle.ToDTO());
		}

		return dtoAlarmList;
	}
}