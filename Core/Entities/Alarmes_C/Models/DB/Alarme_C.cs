using Core.Entities.AlarmesTR.Models.DB;
using Core.Entities.Journals.Models.DB;
using System.ComponentModel.DataAnnotations;

namespace Core.Entities.Alarmes_C.Models.DB
{
    public partial class Alarme_C
    {
        [Key]
        public int IdAlarm { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public virtual ICollection<Journal> Journaux { get; set; }

        public virtual AlarmeTR AlarmeTR { get; set; }

    }
}
