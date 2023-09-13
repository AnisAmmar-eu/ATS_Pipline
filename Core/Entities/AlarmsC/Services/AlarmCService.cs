using System.Linq.Expressions;
using Core.Entities.AlarmsC.Models.DB;
using Core.Entities.AlarmsC.Models.DTO;
using Core.Shared.Exceptions;
using Core.Shared.UnitOfWork.Interfaces;

namespace Core.Entities.AlarmsC.Services;

public class AlarmCService : IAlarmCService
{
	private readonly IAlarmUOW _alarmUOW;

	public AlarmCService(IAlarmUOW alarmUOW)
	{
		_alarmUOW = alarmUOW;
	}

	/// <summary>
	/// Get an AlarmC by its ID
	/// </summary>
	/// <param name="ID"></param>
	/// <returns></returns>
	public async Task<DTOAlarmC> GetById(int ID)
	{
		return (await _alarmUOW.AlarmC.GetById(ID)).ToDTO();
	}

	/// <summary>
	/// Search for an AlarmC based on its RID.
	/// </summary>
	/// <param name="RID"></param>
	/// <returns></returns>
	public async Task<DTOAlarmC> GetByRID(string RID)
	{
		return (await _alarmUOW.AlarmC.GetBy(new Expression<Func<AlarmC, bool>>[]
		{
			alarm => alarm.RID == RID
		})).ToDTO();
	}

	/// <summary>
	/// Tries to add a received AlarmC to AlarmsServer, if it already exists, it returns the AlarmC from the DB.
	/// If it doesn't, creates a new one and returns it. Comparison is made with the RID.
	/// </summary>
	/// <param name="received">Received AlarmC from a station</param>
	/// <returns>An already existing AlarmC with the same RID or a new one.</returns>
	public async Task<DTOAlarmC> AddReceivedAlarmC(DTOAlarmC received)
	{
		try
		{
			DTOAlarmC alarmC = await GetByRID(received.RID);
			return alarmC;
		}
		catch (EntityNotFoundException e)
		{
			await _alarmUOW.StartTransaction();
			AlarmC model = received.ToModel();
			model.ID = 0;
			await _alarmUOW.AlarmC.Add(model);
			_alarmUOW.Commit();
			await _alarmUOW.CommitTransaction();
			return model.ToDTO();
		}
	}
}