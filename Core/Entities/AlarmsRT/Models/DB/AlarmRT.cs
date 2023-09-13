using Core.Entities.AlarmsC.Models.DB;
using Core.Shared.Models.DB.Kernel;
using Core.Shared.Models.DB.Kernel.Interfaces;
using DTOAlarmRT = Core.Entities.AlarmsRT.Models.DTO.DTOAlarmRT;

namespace Core.Entities.AlarmsRT.Models.DB;

public partial class AlarmRT : BaseEntity, IBaseEntity<AlarmRT, DTOAlarmRT>
{
    public int IDAlarm { get; set; }
    public virtual AlarmC AlarmC { get; set; }


    public int? Status { get; set; }

    public string? Station { get; set; }
    public int? NumberNonRead { get; set; }
}