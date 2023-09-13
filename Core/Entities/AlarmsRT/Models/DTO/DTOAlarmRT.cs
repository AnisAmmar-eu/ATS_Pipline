using Core.Entities.AlarmsC.Models.DB;
using Core.Shared.Models.DTOs.Kernel;
using Core.Shared.Models.DTOs.Kernel.Interfaces;

namespace Core.Entities.AlarmsRT.Models.DTO;

public partial class DTOAlarmRT : DTOBaseEntity, IDTO<AlarmsRT.Models.DB.AlarmRT, AlarmsRT.Models.DTO.DTOAlarmRT>
{
    public int IDAlarm { get; set; }
    public AlarmC AlarmC { get; set; }
    public int? Status { get; set; }
    public string? Station { get; set; }
    public int? NumberNonRead { get; set; }
}