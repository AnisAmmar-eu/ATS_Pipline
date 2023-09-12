using Core.Entities.AlarmesPLC.Models.DTO;
using Core.Shared.Models.DB.Kernel;
using Core.Shared.Models.DB.Kernel.Interfaces;

namespace Core.Entities.AlarmesPLC.Models.DB
{
    public partial class AlarmePLC : BaseEntity, IBaseEntity<AlarmePLC, DTOAlarmePLC>
    {
        public DTOAlarmePLC ToDTO(string? languageRID = null)
        {
            return new DTOAlarmePLC(this);
        }
    }
}
