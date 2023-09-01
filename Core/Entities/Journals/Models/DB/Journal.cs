using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Core.Entities.Alarmes_C.Models.DB;
using Core.Entities.Journals.Models.DTOs;

namespace Core.Entities.Journals.Models.DB
{
    public partial class Journal
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Alarme")]
        public int IdAlarme { get; set; }
        public virtual Alarme_C Alarme { get; set; }

        public int? Status1 { get; set; }
        public DateTimeOffset? TS1 { get; set; }
        public int? Status0 { get; set; }
        public DateTimeOffset? TS0 { get; set; }
        public DateTimeOffset? TS { get; set; }

        public string? Station { get; set; }
        public int? Lu { get; set; }

        public DateTimeOffset? TSLu { get; set; }


    }
}
