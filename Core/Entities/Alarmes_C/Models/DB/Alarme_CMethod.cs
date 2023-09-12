using Core.Entities.Alarmes_C.Models.DTOs;
using Core.Shared.Models.DB.Kernel;
using Core.Shared.Models.DB.Kernel.Interfaces;

namespace Core.Entities.Alarmes_C.Models.DB
{
    public partial class Alarme_C : BaseEntity, IBaseEntity<Alarme_C, DTOAlarme_C>
    {
        public Alarme_C()
        {
            Name = "";
            Description = "";
        }

        public DTOAlarme_C ToDTO(string? languageRID = null)
        {
            return new();
        }
    }
}
