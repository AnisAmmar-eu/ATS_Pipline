using System.ComponentModel.DataAnnotations;

namespace Core.Entities.AlarmesPLC.Models.DB
{
    public partial class AlarmePLC
    {
        [Key]
        public int Id { get; set; }
        public int IdAlarme { get; set; }
        public int Status { get; set; }
        
        public DateTimeOffset TS { get; set; }
    
    }
}
