using Core.Entities.Packets.Dictionaries;
using Core.Entities.Packets.Models.DB.AlarmLists;
using Mapster;

namespace Core.Entities.Packets.Models.DTO.AlarmLists;

public partial class DTOAlarmList
{
	public DTOAlarmList()
	{
		Type = PacketTypes.AlarmList;
		AlarmCycles = [];
	}

	public override AlarmList ToModel() => this.Adapt<AlarmList>();
}