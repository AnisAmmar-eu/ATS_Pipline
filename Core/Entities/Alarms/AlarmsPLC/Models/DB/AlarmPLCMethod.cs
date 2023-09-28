﻿using Core.Shared.Models.DB.Kernel;
using Core.Shared.Models.DB.Kernel.Interfaces;
using Core.Entities.Alarms.AlarmsPLC.Models.DTO;
namespace Core.Entities.Alarms.AlarmsPLC.Models.DB;

public partial class AlarmPLC : BaseEntity, IBaseEntity<Alarms.AlarmsPLC.Models.DB.AlarmPLC, DTOAlarmPLC>
{
	public AlarmPLC()
	{}
	public AlarmPLC(Alarm alarm)
	{
		TS = DateTimeOffset.FromUnixTimeSeconds(alarm.TimeStamp).AddMilliseconds(alarm.TimeStampMS);
		AlarmID = (int)alarm.ID;
		IsActive = alarm.Status == 1;
		IsOneShot = alarm.OneShot;
	}
	public DTOAlarmPLC ToDTO(string? languageRID = null)
	{
		return new DTOAlarmPLC(this);
	}
}