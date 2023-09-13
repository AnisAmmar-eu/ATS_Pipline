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
        IDAlarm = alarmC.ID;
        AlarmC = alarmC;
    }
    public AlarmRT(DTOAlarmRT dtoAlarmRT)
    {
        AlarmC = dtoAlarmRT.AlarmC;
        ID = dtoAlarmRT.ID;
        TS = (DateTimeOffset)dtoAlarmRT.TS!;
        IDAlarm = dtoAlarmRT.IDAlarm;
        Status = dtoAlarmRT.Status;
        Station = dtoAlarmRT.Station;
        NumberNonRead = dtoAlarmRT.NumberNonRead;
    }
    public override DTOAlarmRT ToDTO(string? languageRID = null)
    {
        return new DTOAlarmRT(this, languageRID);
    }
}