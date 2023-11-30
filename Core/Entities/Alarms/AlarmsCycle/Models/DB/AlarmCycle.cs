using Core.Entities.Alarms.AlarmsCycle.Models.DTO;
using Core.Entities.Packets.Models.DB.AlarmLists;
using Core.Shared.Models.DB.Kernel;
using Core.Shared.Models.DB.Kernel.Interfaces;

namespace Core.Entities.Alarms.AlarmsCycle.Models.DB;

public partial class AlarmCycle : BaseEntity, IBaseEntity<AlarmCycle, DTOAlarmCycle>
{
	public string AlarmRID { get; set; }
	public int? NbNonAck { get; set; }
	public bool IsActive { get; set; }

	public int AlarmListPacketID { get; set; }

	#region Nav Properties

	private AlarmList? _alarmListPacket;

	public AlarmList AlarmList
	{
		set => _alarmListPacket = value;
		get => _alarmListPacket
			?? throw new InvalidOperationException("Uninitialized property: " + nameof(AlarmList));
	}

	#endregion
}