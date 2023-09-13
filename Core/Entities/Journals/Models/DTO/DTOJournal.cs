using Core.Entities.AlarmsC.Models.DB;
using Core.Entities.Journals.Models.DB;
using Core.Shared.Models.DTOs.Kernel;
using Core.Shared.Models.DTOs.Kernel.Interfaces;

namespace Core.Entities.Journals.Models.DTO;

public partial class DTOJournal : DTOBaseEntity, IDTO<Journal, DTOJournal>
{
    public int IDAlarm { get; set; }
    public virtual AlarmC? Alarm { get; set; }

    public int? Status1 { get; set; }
    public DateTimeOffset? TS1 { get; set; }
    public int? Status0 { get; set; }
    public DateTimeOffset? TS0 { get; set; }
    public string? Station { get; set; }
    public int? IsRead { get; set; }
}