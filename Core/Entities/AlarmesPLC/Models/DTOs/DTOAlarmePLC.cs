using System.ComponentModel.DataAnnotations;

namespace Core.Entities.AlarmesPLC.Models.DTO
{
    public partial class DTOAlarmePLC
    {
        [Key]
        public int Id { get; set; }
        public int IdAlarme { get; set; }
        public int Status { get; set; }
        
        public DateTimeOffset TS { get; set; }
    
    }
}
