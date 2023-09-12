using Core.Entities.AlarmesTR.Models.DB;
using Core.Entities.Journals.Models.DB;
using System.ComponentModel.DataAnnotations;
using Core.Entities.Alarmes_C.Models.DTOs;
using Core.Shared.Models.DB.Kernel;
using Core.Shared.Models.DB.Kernel.Interfaces;

namespace Core.Entities.Alarmes_C.Models.DB
{
    public partial class Alarme_C : BaseEntity, IBaseEntity<Alarme_C, DTOAlarme_C>
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public virtual ICollection<Journal> Journaux { get; set; }

        public virtual AlarmeTR AlarmeTR { get; set; }

    }
}
