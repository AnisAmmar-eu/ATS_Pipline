using Core.Entities.AlarmsC.Models.DB;
using Core.Entities.Journals.Models.DTO;
using Core.Shared.Models.DB.Kernel;
using Core.Shared.Models.DB.Kernel.Interfaces;

namespace Core.Entities.Journals.Models.DB;

public partial class Journal : BaseEntity, IBaseEntity<Journal, DTOJournal>
{
    public int IDAlarm { get; set; }
    public virtual AlarmC Alarm { get; set; }

    public int? Status1 { get; set; }
    public DateTimeOffset? TS1 { get; set; }
    public int? Status0 { get; set; }
    public DateTimeOffset? TS0 { get; set; }

    public string? Station { get; set; }
    public int? IsRead { get; set; }

    public DateTimeOffset? TSRead { get; set; }
}