﻿using System.ComponentModel.DataAnnotations.Schema;
using Core.Entities.Alarms.AlarmsC.Models.DB;
using Core.Entities.Alarms.AlarmsLog.Models.DTO;
using Core.Shared.Dictionaries;
using Core.Shared.Models.DB.Kernel;
using Core.Shared.Models.DB.Kernel.Interfaces;

namespace Core.Entities.Alarms.AlarmsLog.Models.DB;

public partial class AlarmLog : BaseEntity, IBaseEntity<AlarmLog, DTOAlarmLog>
{
	public bool HasBeenSent { get; set; }
	public int AlarmID { get; set; }
	public int StationID { get; set; } = Station.ID;

	/// <summary>
	///     Ack = Acknowledge
	/// </summary>
	public bool IsAck { get; set; }

	public bool IsActive { get; set; }
	public DateTimeOffset TSRaised { get; set; }
	public DateTimeOffset? TSClear { get; set; }
	public TimeSpan? Duration { get; set; }
	public DateTimeOffset? TSRead { get; set; }
	public DateTimeOffset? TSGet { get; set; }

	/// <summary>
	///     Only used in Collect() as AlarmC is not yet known.
	/// </summary>
	[NotMapped]
	public string? RID { get; set; } = string.Empty;

	private AlarmC? _alarm;

	public AlarmC Alarm
	{
		set => _alarm = value;
		get => _alarm ?? throw new InvalidOperationException("Uninitialized property: " + nameof(Alarm));
	}
}