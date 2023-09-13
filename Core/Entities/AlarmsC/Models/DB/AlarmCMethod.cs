using Core.Entities.AlarmsC.Models.DTO;
using Core.Shared.Models.DB.Kernel;
using Core.Shared.Models.DB.Kernel.Interfaces;

namespace Core.Entities.AlarmsC.Models.DB;

public partial class AlarmC : BaseEntity, IBaseEntity<AlarmC, DTOAlarmC>
{
    public AlarmC()
    {
        Name = "";
        Description = "";
    }

    public override DTOAlarmC ToDTO(string? languageRID = null)
    {
        return new DTOAlarmC(this);
    }
}