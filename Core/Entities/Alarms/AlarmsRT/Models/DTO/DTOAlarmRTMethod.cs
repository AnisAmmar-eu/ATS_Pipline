using Core.Shared.Models.DTOs.Kernel;
using Core.Shared.Models.DTOs.Kernel.Interfaces;
using Core.Entities.Alarms.AlarmsRT.Models.DB;
namespace Core.Entities.Alarms.AlarmsRT.Models.DTO;

public partial class DTOAlarmRT : DTOBaseEntity, IDTO<AlarmRT, Alarms.AlarmsRT.Models.DTO.DTOAlarmRT>
{
	public DTOAlarmRT(AlarmRT alarmRT, string languageRID)
	{
		ID = alarmRT.ID;
		TS = alarmRT.TS;
		IRID = alarmRT.IRID;
		AlarmID = alarmRT.AlarmID;
		Station = alarmRT.Station;
		NbNonAck = alarmRT.NbNonAck;
		IsActive = alarmRT.IsActive;
		TSRaised = alarmRT.TSRaised;
		TSClear = alarmRT.TSClear;
		
		Alarm = alarmRT.Alarm.ToDTO();
	}

	public override AlarmRT ToModel()
	{
		return new AlarmRT(this);
	}
}