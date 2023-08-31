using Core.Entities.Alarmes_C.Models.DB;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Core.Entities.Journals.Models.DTOs
{
    public partial class DTOJournal
    {
        public int Id { get; set; }

        public int IdAlarme { get; set; }
        public virtual Alarme_C? Alarme { get; set; }

        public int? Status1 { get; set; }
        public DateTimeOffset? TS1 { get; set; }
        public int? Status0 { get; set; }
        public DateTimeOffset? TS0 { get; set; }
        public DateTimeOffset? TS { get; set; }

        public string? Station { get; set; }
        public int? Lu { get; set; }
    }
}
