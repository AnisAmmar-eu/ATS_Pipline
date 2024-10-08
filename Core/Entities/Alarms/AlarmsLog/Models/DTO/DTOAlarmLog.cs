﻿using Core.Entities.Alarms.AlarmsC.Models.DTO;
using Core.Entities.Alarms.AlarmsLog.Models.DB;
using Core.Shared.Dictionaries;
using Core.Shared.Models.DTO.Kernel;
using Core.Shared.Models.DTO.Kernel.Interfaces;

namespace Core.Entities.Alarms.AlarmsLog.Models.DTO;

public partial class DTOAlarmLog : DTOBaseEntity, IDTO<AlarmLog, DTOAlarmLog>
{
	public int StationID { get; set; } = Station.ID;
	public bool IsAck { get; set; }
	public bool IsActive { get; set; }
	public bool HasBeenSent { get; set; }
	public int AlarmID { get; set; }
	public string AlarmRID { get; set; } = string.Empty;
	public DTOAlarmC Alarm { get; set; }
	public DateTimeOffset TSRaised { get; set; }
	public DateTimeOffset? TSClear { get; set; }
	public TimeSpan? Duration { get; set; }
	public DateTimeOffset? TSRead { get; set; }
	public DateTimeOffset? TSGet { get; set; }
}