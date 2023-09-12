﻿using Core.Entities.AlarmesPLC.Models.DB;
using Core.Shared.Models.DTOs.Kernel;
using Core.Shared.Models.DTOs.Kernel.Interfaces;

namespace Core.Entities.AlarmesPLC.Models.DTO
{
    public partial class DTOAlarmePLC : DTOBaseEntity, IDTO<AlarmePLC, DTOAlarmePLC>
    {
        public DTOAlarmePLC(AlarmePLC alarmePLC)
        {
            ID = alarmePLC.ID;
            TS = alarmePLC.TS;
            IdAlarme = alarmePLC.IdAlarme;
            Status = alarmePLC.Status;
        }
    }
}
