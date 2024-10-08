﻿using Core.Entities.Alarms.AlarmsC.Models.DTO;
using Core.Entities.Alarms.AlarmsLog.Models.DB;
using Core.Entities.Alarms.AlarmsRT.Models.DB;
using Core.Shared.Models.DB.Kernel;
using Core.Shared.Models.DB.Kernel.Interfaces;

namespace Core.Entities.Alarms.AlarmsC.Models.DB;

public partial class AlarmC : BaseEntity, IBaseEntity<AlarmC, DTOAlarmC>
{
	public string RID { get; set; } = string.Empty;
	public string Name { get; set; } = string.Empty;
	public string Description { get; set; } = string.Empty;
	public string Category { get; set; } = string.Empty;
	public int Severity { get; set; }

	#region Nav Properties
	public virtual ICollection<AlarmRT> AlarmRTs { get; set; } = new List<AlarmRT>();

	public virtual ICollection<AlarmLog> AlarmLogs { get; set; } = new List<AlarmLog>();

	#endregion Nav Properties
}