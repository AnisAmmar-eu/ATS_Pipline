using System.Text;
using System.Text.Json;
using Core.Entities.Alarms.AlarmsCycle.Models.DB;
using Core.Entities.Alarms.AlarmsCycle.Models.DTO;
using Core.Entities.Alarms.AlarmsRT.Models.DB;
using Core.Entities.IOT.Dictionaries;
using Core.Entities.Packets.Dictionaries;
using Core.Entities.Packets.Models.DTO.AlarmLists;
using Core.Shared.UnitOfWork.Interfaces;

namespace Core.Entities.Packets.Models.DB.AlarmLists;

public partial class AlarmList
{
	public AlarmList()
	{
		AlarmCycles = new List<AlarmCycle>();
	}

	public AlarmList(DTOAlarmList dto) : base(dto)
	{
		AlarmCycles = dto.AlarmCycles.ToList().ConvertAll(dtoAlarmCycle =>
		{
			AlarmCycle alarmCycle = dtoAlarmCycle.ToModel();
			alarmCycle.AlarmList = this;
			return alarmCycle;
		});
	}

	public override DTOAlarmList ToDTO()
	{
		return new(this);
	}

	public async Task SendAlarmsCycle(IAnodeUOW anodeUOW, int stationCycleID)
	{
		using HttpClient httpClient = new();

		List<DTOAlarmCycle> alarmsCycle = AlarmCycles.ToList().ConvertAll(cycle => cycle.ToDTO());
		alarmsCycle.ForEach(cycle => cycle.AlarmListPacketID = stationCycleID);
		StringContent content
			= new(
				JsonSerializer.Serialize(alarmsCycle),
				Encoding.UTF8,
				"application/json");
		HttpResponseMessage response = await httpClient.PostAsync(
			$"{ITApisDict.ServerReceiveAddress}/apiServerReceive/alarmsCycle",
			content);
		if (!response.IsSuccessStatusCode)
			throw new HttpRequestException("Could not send alarms cycle to the server: " + response.ReasonPhrase);
	}

	protected override async Task InheritedBuild(IAnodeUOW anodeUOW)
	{
		List<AlarmRT> alarmRTs = await anodeUOW.AlarmRT.GetAllWithInclude(withTracking: false);
		foreach (AlarmRT alarmRT in alarmRTs)
		{
			AlarmCycle alarmCycle = new(alarmRT) {
				AlarmListPacketID = ID,
				AlarmList = this,
			};
			await anodeUOW.AlarmCycle.Add(alarmCycle);
			AlarmCycles.Add(alarmCycle);
		}

		if (AlarmCycles.Count != 0)
			anodeUOW.Commit();

		Status = PacketStatus.Completed;
	}
}