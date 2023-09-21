using Core.Entities.AlarmsCycle.Models.DTO;
using Core.Entities.Packets.Models.DB.AlarmListPackets;
using Core.Shared.Models.DB.Kernel;
using Core.Shared.Models.DB.Kernel.Interfaces;

namespace Core.Entities.AlarmsCycle.Models.DB;

public partial class AlarmCycle : BaseEntity, IBaseEntity<AlarmCycle, DTOAlarmCycle>
{
	public string AlarmRID { get; set; }
	public int? NbNonAck { get; set; }
	public bool IsActive { get; set; }
	
	public int AlarmListPacketID { get; set; }
	
	#region Nav Properties

	private AlarmListPacket? _alarmListPacket;

	public AlarmListPacket AlarmListPacket
	{
		set => _alarmListPacket = value;
		get => _alarmListPacket
		       ?? throw new InvalidOperationException("Uninitialized property: " + nameof(AlarmListPacket));
	}

	#endregion
}