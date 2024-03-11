using System.Text;
using System.Text.Json;
using Core.Entities.Alarms.AlarmsC.Models.DB;
using Core.Entities.Alarms.AlarmsLog.Models.DB;
using Core.Entities.Alarms.AlarmsLog.Models.DTO;
using Core.Entities.Alarms.AlarmsLog.Repositories;
using Core.Entities.IOT.Dictionaries;
using Core.Shared.Exceptions;
using Core.Shared.Services.Kernel;
using Core.Shared.SignalR.AlarmHub;
using Core.Shared.UnitOfWork.Interfaces;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace Core.Entities.Alarms.AlarmsLog.Services;

public class AlarmLogService : BaseEntityService<IAlarmLogRepository, AlarmLog, DTOAlarmLog>, IAlarmLogService
{
	private readonly IHubContext<AlarmHub, IAlarmHub> _hubContext;
	private readonly ILogger<AlarmLogService> _logger;

	public AlarmLogService(
		IAnodeUOW anodeUOW,
		IHubContext<AlarmHub, IAlarmHub> hubContext,
		ILogger<AlarmLogService> logger) : base(anodeUOW)
	{
		_hubContext = hubContext;
		_logger = logger;
	}

	/// <summary>
	/// Collects alarm data and processes it based on its current state and value.
	/// This method handles both existing alarms that are currently active and new alarm instances.
	/// </summary>
	/// <param name="alarm">The alarm instance to be processed.</param>
	/// <remarks>
	/// This method performs several key functions:
	/// <list type="number">
	/// <item>
	/// Checks if there is an existing active alarm log for the given alarm.
	///    - If an active alarm log exists and the alarm's value is true, logs a warning and exits.
	///    - If an active alarm log exists and the alarm's value is false, updates the alarm log status to inactive.
	/// </item>
	/// <item>
	/// In case no active alarm log exists and the alarm's value is false, exits the method.
	/// </item>
	/// <item>
	/// If no active alarm log exists and the alarm's value is true, creates a new alarm log.
	///    - The new alarm log is marked as active unless the alarm is a one-shot type.
	/// </item>
	/// <item>
	/// Initiates a transaction for database operations.
	/// </item>
	/// <item>
	/// Commits the transaction and sends real-time updates to all connected clients about the alarm status.
	/// </item>
	/// </list>
	/// </remarks>
	/// <exception cref="EntityNotFoundException">Thrown when no matching alarm is found in the database.</exception>
	/// <returns>
	/// A Task representing the asynchronous operation.
	/// </returns>
	public async Task Collect(Alarm alarm)
	{
		try
		{
			// If an active alarmLog already exists, this alarm is active and waiting to be cleared.
			AlarmLog alarmWithStatus = await AnodeUOW.AlarmLog.GetByWithIncludes(
				[alarmLog => alarmLog.IsActive && alarmLog.Alarm.RID == alarm.RID.ToString()]);
			if (alarm.Value)
				return; // alarmLog is already active.

			alarmWithStatus.IsActive = false;
			alarmWithStatus.TSClear = alarm.TS.GetTimestamp();
			alarmWithStatus.TS = DateTimeOffset.Now;
			alarmWithStatus.HasBeenSent = false;
			await AnodeUOW.StartTransaction();
			AnodeUOW.AlarmLog.Update(alarmWithStatus);
			AnodeUOW.Commit();
			await AnodeUOW.CommitTransaction();
		}
		catch (EntityNotFoundException)
		{
			if (!alarm.Value)
				return; // alarmLog is already inactive or cleared.

			// If an alarmLog doesn't exist, this alarm just raised.
			AlarmC alarmC = await AnodeUOW.AlarmC.GetBy([alarmLog => alarmLog.RID == alarm.RID.ToString()]);
			AlarmLog newAlarmLog = new(alarmC) {
				Alarm = alarmC,
				TS = DateTimeOffset.Now,
				HasBeenSent = false,
			};
			if (alarm.OneShot)
			{
				newAlarmLog.IsActive = false;
				newAlarmLog.TSClear = alarm.TS.GetTimestamp();
			}
			else
			{
				newAlarmLog.IsActive = true;
			}

			newAlarmLog.TSRaised = alarm.TS.GetTimestamp();
			await AnodeUOW.StartTransaction();
			await AnodeUOW.AlarmLog.Add(newAlarmLog);
			AnodeUOW.Commit();
			await AnodeUOW.CommitTransaction();
		}
		catch (Exception e)
		{
			_logger.LogError($"Error while collecting AlarmLog: {e}");
			return;
		}
	}

	public async Task<int> AckAlarmLogs(int[] idAlarmLogs)
	{
		await AnodeUOW.StartTransaction();
		int res = await AnodeUOW.AlarmLog.AckAlarmLogs(idAlarmLogs);
		AnodeUOW.Commit();
		await AnodeUOW.CommitTransaction();
		await _hubContext.Clients.All.RefreshAlarmRT();
		await _hubContext.Clients.All.RefreshAlarmLog();
		return res;
	}

	// TODO Send one by one for exception control instead of a list.
	public async Task SendLogsToServer()
	{
		try
		{
			string api2Url = $"{ITApisDict.ServerReceiveAddress}/apiServerReceive/alarmsLog";
			AlarmLog alarmLog = await AnodeUOW.AlarmLog.GetByWithIncludes(
				[alarmLog => !alarmLog.HasBeenSent],
				query => query.OrderByDescending(alarmLog => alarmLog.ID),
				false);

			List<AlarmLog> alarmLogs = [ alarmLog ];
			string jsonData = JsonSerializer.Serialize(alarmLogs.ConvertAll(alarmLog => alarmLog.ToDTO()));
			StringContent content = new(jsonData, Encoding.UTF8, "application/json");
			using HttpClient httpClient = new();
			HttpResponseMessage response = await httpClient.PostAsync(api2Url, content);

			if (!response.IsSuccessStatusCode)
			{
				throw new("Send alarmLog to server failed with status code:"
					+ $" {response.StatusCode.ToString()}\nReason: {response.ReasonPhrase}");
			}

			await AnodeUOW.AlarmLog.ExecuteUpdateByIdAsync(
				alarmLog,
				setters => setters.SetProperty(alarmLog => alarmLog.HasBeenSent, true));

			await _hubContext.Clients.All.RefreshAlarmRT();
			await _hubContext.Clients.All.RefreshAlarmLog();
		}
		catch (Exception e)
		{
			_logger.LogError($"Error while sending AlarmLog: {e}");
		}

		await AnodeUOW.CommitTransaction();
	}

	public async Task ReceiveAlarmLog(DTOAlarmLog dtoAlarmLog)
	{
		try
		{
			AlarmLog alarmWithStatus = await AnodeUOW.AlarmLog.GetByWithIncludes(
				[alarmLog => alarmLog.IsActive && alarmLog.Alarm.RID == dtoAlarmLog.AlarmRID],
				query => query.OrderByDescending(alarmLog => alarmLog.ID),
				false);

			if (dtoAlarmLog.IsActive)
				return; // alarmLog is already active.

			alarmWithStatus.IsActive = false;
			alarmWithStatus.TSClear = dtoAlarmLog.TSClear;
			if (dtoAlarmLog.TS.HasValue)
				alarmWithStatus.TS = dtoAlarmLog.TS.Value;

			alarmWithStatus.IsAck = false;
			alarmWithStatus.HasBeenSent = false;
			await AnodeUOW.StartTransaction();
			AnodeUOW.AlarmLog.Update(alarmWithStatus);
			AnodeUOW.Commit();
			await AnodeUOW.CommitTransaction();
		}
		catch (EntityNotFoundException)
		{
			// If an alarmLog doesn't exist, this alarm just raised.
			AlarmLog newAlarmLog = dtoAlarmLog.ToModel();
			newAlarmLog.Alarm = await AnodeUOW.AlarmC.GetBy([alarmC => alarmC.RID == dtoAlarmLog.AlarmRID]);
			newAlarmLog.ID = 0;
			await AnodeUOW.StartTransaction();
			await AnodeUOW.AlarmLog.Add(newAlarmLog);
			AnodeUOW.Commit();
			await AnodeUOW.CommitTransaction();
		}
	}
}