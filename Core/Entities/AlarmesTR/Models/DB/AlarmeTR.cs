using Core.Entities.Alarmes_C.Models.DB;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities.AlarmesTR.Models.DB
{
   public partial class AlarmeTR
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Alarme_C")]
        public int IdAlarme { get; set; }
        public virtual Alarme_C Alarme_C { get; set; }


        public int? Status { get; set; }

        public DateTimeOffset? TS { get; set; }

        public string? Station { get; set; }
        public int? NombreNonLu { get; set; }
    }
}
