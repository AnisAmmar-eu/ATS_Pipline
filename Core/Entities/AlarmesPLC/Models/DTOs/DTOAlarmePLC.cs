using System.ComponentModel.DataAnnotations;
using Core.Entities.AlarmesPLC.Models.DB;
using Core.Shared.Models.DTOs.Kernel;
using Core.Shared.Models.DTOs.Kernel.Interfaces;

namespace Core.Entities.AlarmesPLC.Models.DTO
{
    public partial class DTOAlarmePLC : DTOBaseEntity, IDTO<AlarmePLC, DTOAlarmePLC>
    {
        public int IdAlarme { get; set; }
        public int Status { get; set; }
    }
}
