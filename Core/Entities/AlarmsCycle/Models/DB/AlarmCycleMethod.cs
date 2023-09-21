using Core.Entities.AlarmsCycle.Models.DTO;
using Core.Entities.AlarmsRT.Models.DB;
using Core.Shared.Models.DB.Kernel;
using Core.Shared.Models.DB.Kernel.Interfaces;

namespace Core.Entities.AlarmsCycle.Models.DB;

public partial class AlarmCycle : BaseEntity, IBaseEntity<AlarmCycle, DTOAlarmCycle>
{
	public AlarmCycle()
	{
		AlarmRID = "";
	}
	public AlarmCycle(AlarmRT alarmRT)
	{
		AlarmRID = alarmRT.Alarm.RID;
		NbNonAck = alarmRT.NbNonAck;
		IsActive = alarmRT.IsActive;
	}

	public AlarmCycle(DTOAlarmCycle dtoAlarmCycle)
	{
		AlarmRID = dtoAlarmCycle.AlarmRID;
		NbNonAck = dtoAlarmCycle.NbNonAck;
		IsActive = dtoAlarmCycle.IsActive;
		AlarmListPacketID = dtoAlarmCycle.AlarmListPacketID;
	}
	public override DTOAlarmCycle ToDTO(string? languageRID = null)
	{
		return new DTOAlarmCycle(this);
	}
}