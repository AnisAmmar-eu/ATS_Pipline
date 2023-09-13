using Core.Entities.AlarmsPLC.Models.DTOs;
using Core.Shared.Models.DB.Kernel;
using Core.Shared.Models.DB.Kernel.Interfaces;

namespace Core.Entities.AlarmsPLC.Models.DB;

public partial class AlarmPLC : BaseEntity, IBaseEntity<AlarmPLC, DTOAlarmPLC>
{
    public DTOAlarmPLC ToDTO(string? languageRID = null)
    {
        return new DTOAlarmPLC(this);
    }
}