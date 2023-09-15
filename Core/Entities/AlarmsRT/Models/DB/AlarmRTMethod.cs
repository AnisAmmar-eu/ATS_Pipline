using Core.Entities.AlarmsC.Models.DB;
using Core.Shared.Models.DB.Kernel;
using Core.Shared.Models.DB.Kernel.Interfaces;
using DTOAlarmRT = Core.Entities.AlarmsRT.Models.DTO.DTOAlarmRT;

namespace Core.Entities.AlarmsRT.Models.DB;

public partial class AlarmRT : BaseEntity, IBaseEntity<AlarmsRT.Models.DB.AlarmRT, DTOAlarmRT>
{
	public AlarmRT()
	{
	}

	public AlarmRT(AlarmC alarmC)
	{
		AlarmID = alarmC.ID;
		Alarm = alarmC;
	}

	public AlarmRT(DTOAlarmRT dtoAlarmRT)
	{
		ID = dtoAlarmRT.ID;
		TS = (DateTimeOffset)dtoAlarmRT.TS!;
		IRID = dtoAlarmRT.IRID;
		AlarmID = dtoAlarmRT.AlarmID;
		Station = dtoAlarmRT.Station;
		NbNonAck = dtoAlarmRT.NbNonAck;
		IsActive = dtoAlarmRT.IsActive;
		TSRaised = dtoAlarmRT.TSRaised;
		TSClear = dtoAlarmRT.TSClear;
		
		Alarm = dtoAlarmRT.Alarm;
	}

	public override DTOAlarmRT ToDTO(string? languageRID = null)
	{
		return new DTOAlarmRT(this, languageRID);
	}
}