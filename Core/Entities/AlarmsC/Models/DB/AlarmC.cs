using Core.Entities.AlarmsC.Models.DTO;
using Core.Entities.AlarmsRT.Models.DB;
using Core.Entities.Journals.Models.DB;
using Core.Shared.Models.DB.Kernel;
using Core.Shared.Models.DB.Kernel.Interfaces;

namespace Core.Entities.AlarmsC.Models.DB;

public partial class AlarmC : BaseEntity, IBaseEntity<AlarmC, DTOAlarmC>
{
    public string Name { get; set; }
    public string Description { get; set; }

    public virtual ICollection<Journal> Journals { get; set; }

    public virtual AlarmRT AlarmRT { get; set; }
}