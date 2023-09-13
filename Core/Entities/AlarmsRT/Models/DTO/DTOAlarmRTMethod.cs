using Core.Entities.AlarmsRT.Models.DB;
using Core.Shared.Models.DTOs.Kernel;
using Core.Shared.Models.DTOs.Kernel.Interfaces;

namespace Core.Entities.AlarmsRT.Models.DTO;

public partial class DTOAlarmRT : DTOBaseEntity, IDTO<AlarmRT, DTOAlarmRT>
{
    public DTOAlarmRT(AlarmRT alarmRT, string languageRID)
    {
        ID = alarmRT.ID;
        TS = alarmRT.TS;
        IDAlarm = alarmRT.IDAlarm;
        Status = alarmRT.Status;
        Station = alarmRT.Station;
        NumberNonRead = alarmRT.NumberNonRead;
    }
    
    public override AlarmRT ToModel()
    {
        return new AlarmRT(this);
    }
}