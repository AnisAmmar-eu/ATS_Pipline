using System.Linq.Expressions;
using System.Text;
using System.Text.Json;
using Core.Entities.Alarms.AlarmsC.Models.DB;
using Core.Entities.Alarms.AlarmsRT.Models.DB;
using Core.Entities.Alarms.AlarmsRT.Models.DTO;
using Core.Entities.Alarms.AlarmsRT.Repositories;
using Core.Entities.IOT.Dictionaries;
using Core.Shared.Exceptions;
using Core.Shared.Services.Kernel;
using Core.Shared.SignalR.AlarmHub;
using Core.Shared.UnitOfWork.Interfaces;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using TwinCAT.Ads;

namespace Core.Entities.Alarms.AlarmsRT.Services;

public class AlarmRTService : BaseEntityService<IAlarmRTRepository, AlarmRT, DTOAlarmRT>, IAlarmRTService
{
	private readonly IHubContext<AlarmHub, IAlarmHub> _hubContext;
	private readonly ILogger<AlarmRTService> _logger;

	public AlarmRTService(
		IAnodeUOW anodeUOW,
		IHubContext<AlarmHub, IAlarmHub> hubContext,
		ILogger<AlarmRTService> logger) : base(anodeUOW)
	{
		_hubContext = hubContext;
		_logger = logger;
	}

	public async Task Collect(AdsClient adsClient, uint alarmListHandle)
	{
		try
		{
			Alarm[] alarmStructs = (Alarm[])adsClient.ReadAny(alarmListHandle, typeof(Alarm[]), [41]);
			List<AlarmRT> alarms = await AnodeUOW.AlarmRT.GetAll(withTracking: false);
			foreach (Alarm alarmStruct in alarmStructs)
			{
				AlarmRT alarmRT = new(alarmStruct);
				_logger.LogInformation($"AlarmRT: {alarmRT.TSRaised.ToString()}");
				string RIDAlarmStruct = alarmStruct.RID.ToString();

				try
				{
					if (await AnodeUOW.AlarmRT
						.ExecuteUpdateAsync(
							alarm => alarm.IRID == alarmRT.IRID,
							s => s
								.SetProperty(a => a.IsActive, alarmRT.IsActive)
								.SetProperty(a => a.TSRaised, alarmRT.TSRaised)
								.SetProperty(a => a.TS, DateTimeOffset.Now)
						) == 0)
					{
						throw new EntityNotFoundException();
					}

					alarms = alarms.Where(alarm => alarm.IRID != RIDAlarmStruct).ToList();
				}
				catch (EntityNotFoundException)
				{
					try
					{
						alarmRT.Alarm = await AnodeUOW.AlarmC.GetBy([alarmC => alarmC.RID == RIDAlarmStruct]);
						await AnodeUOW.AlarmRT.Add(alarmRT);
						AnodeUOW.Commit();
						alarms = alarms.Where(alarm => alarm.IRID != RIDAlarmStruct).ToList();
					}
					catch (EntityNotFoundException)
					{
						_logger.LogWarning($"Alarm {RIDAlarmStruct} | {alarmStruct.RID.ToString()} is not in the alarm list.");
					}
				}
			}

			await AnodeUOW.AlarmRT.ExecuteDeleteAsync(alarm => alarms.Select(alarm => alarm.IRID).Contains(alarm.IRID));
			await _hubContext.Clients.All.RefreshAlarmRT();
		}
		catch (Exception e)
		{
			_logger.LogError($"Error while reading alarm list: {e}");
		}
	}

	public async Task SendRTsToServer()
	{
		try
		{
			string api2Url = $"{ITApisDict.ServerReceiveAddress}/apiServerReceive/alarmsRT";
			List<AlarmRT> alarmRTs = await AnodeUOW.AlarmRT.GetAll(withTracking: false);

			string jsonData = JsonSerializer.Serialize(alarmRTs.ConvertAll(alarmRT => alarmRT.ToDTO()));
			StringContent content = new(jsonData, Encoding.UTF8, "application/json");
			using HttpClient httpClient = new();
			HttpResponseMessage response = await httpClient.PostAsync(api2Url, content);

			if (!response.IsSuccessStatusCode)
			{
				throw new("Send alarmRT to server failed with status code:"
					+ $" {response.StatusCode.ToString()}\nReason: {response.ReasonPhrase}");
			}

			await _hubContext.Clients.All.RefreshAlarmRT();
		}
		catch (Exception e)
		{
			_logger.LogError($"Error while sending AlarmRT: {e}");
		}

		await AnodeUOW.CommitTransaction();
	}

	new public async Task<List<DTOAlarmRT>> GetAll(
		Expression<Func<AlarmRT, bool>>[]? filters = null,
		Func<IQueryable<AlarmRT>, IOrderedQueryable<AlarmRT>>? orderBy = null,
		bool withTracking = true,
		int? maxCount = null,
		params string[] includes)
	{
		return (await AnodeUOW.AlarmRT.GetAllWithInclude(filters, orderBy, withTracking)).ConvertAll(alarmRT =>
			alarmRT.ToDTO());
	}

	public async Task<int[]> GetAlarmRTStats()
	{
		List<AlarmRT> alarmRts = await AnodeUOW.AlarmRT.GetAll(withTracking: false);
		List<AlarmC> alarmCs = await AnodeUOW.AlarmC.GetAll(withTracking: false);
		int nbActiveAlarms = alarmRts.Count(alarmRT => alarmRT.IsActive);
		int nbNonAck = (await AnodeUOW.AlarmLog.GetAll([alarmLog => !alarmLog.IsAck], withTracking: false)).Count;
		return [0, nbNonAck, nbActiveAlarms];
	}

	public async Task ReceiveAlarmRT(DTOAlarmRT dtoAlarmRT)
	{
		_logger.LogInformation($"Receiving AlarmRT: {dtoAlarmRT.IRID} | {dtoAlarmRT.StationID.ToString()}");
		List<AlarmRT> alarms = await AnodeUOW.AlarmRT.GetAll(withTracking: false);
		try
		{
			if (await AnodeUOW.AlarmRT
				.ExecuteUpdateAsync(
					a => a.IRID == dtoAlarmRT.IRID && a.StationID == dtoAlarmRT.StationID,
					s => s
						.SetProperty(a => a.IsActive, dtoAlarmRT.IsActive)
						.SetProperty(a => a.TSRaised, dtoAlarmRT.TSRaised)
					) == 0)
			{
				throw new EntityNotFoundException();
			}

			alarms = alarms.Where(alarm => alarm.IRID != dtoAlarmRT.IRID).ToList();
		}
		catch (EntityNotFoundException)
		{
			// If an alarmRT doesn't exist, this alarm just raised.
			AlarmRT newAlarmRT = dtoAlarmRT.ToModel();
			newAlarmRT.ID = 0;
			newAlarmRT.Alarm = await AnodeUOW.AlarmC.GetBy([alarmC => alarmC.RID == newAlarmRT.IRID]);
			await AnodeUOW.StartTransaction();
			await AnodeUOW.AlarmRT.Add(newAlarmRT);
			AnodeUOW.Commit();
			await AnodeUOW.CommitTransaction();
		}
	}
}