using Core.Entities.Alarms.AlarmsC.Repositories;
using Core.Entities.Alarms.AlarmsCycle.Models.Repositories;
using Core.Entities.Alarms.AlarmsLog.Repositories;
using Core.Entities.Alarms.AlarmsPLC.Repositories;
using Core.Entities.Alarms.AlarmsRT.Repositories;
using Core.Entities.Packets.Repositories;
using Core.Shared.Data;
using Core.Shared.UnitOfWork.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;

namespace Core.Shared.UnitOfWork;

public class AlarmUOW : IAlarmUOW
{
	private readonly AlarmCTX _alarmCTX;
	private IDbContextTransaction? _transaction;
	private int _transactionCount;

	public AlarmUOW(AlarmCTX alarmCTX)
	{
		_alarmCTX = alarmCTX;

		AlarmC = new AlarmCRepository(_alarmCTX);
		AlarmPLC = new AlarmPLCRepository(_alarmCTX);
		AlarmLog = new AlarmLogRepository(_alarmCTX);
		AlarmRT = new AlarmRTRepository(_alarmCTX);
		Packet = new PacketRepository(_alarmCTX);
		AlarmCycle = new AlarmCycleRepository(_alarmCTX);
	}


	public IAlarmCRepository AlarmC { get; }
	public IAlarmPLCRepository AlarmPLC { get; }
	public IAlarmLogRepository AlarmLog { get; }
	public IAlarmRTRepository AlarmRT { get; }
	public IPacketRepository Packet { get; }
	public IAlarmCycleRepository AlarmCycle { get; }

	public object? GetRepoByType(Type repo)
	{
		return repo switch
		{
			_ when repo == typeof(IAlarmCRepository) => AlarmC,
			_ when repo == typeof(IAlarmPLCRepository) => AlarmPLC,
			_ when repo == typeof(IAlarmLogRepository) => AlarmLog,
			_ when repo == typeof(IAlarmRTRepository) => AlarmRT,
			_ when repo == typeof(IPacketRepository) => Packet,
			_ when repo == typeof(IAlarmCycleRepository) => Packet,
			_ => null
		};
	}

	public int Commit()
	{
		try
		{
			return _alarmCTX.SaveChangesAsync().Result;
		}
		catch (Exception e)
		{
			if (_transaction != null)
			{
				_transaction.Rollback();
				_transaction = null;
			}

			throw new Exception("An error happened during SaveChanges", e);
		}
	}

	/// <summary>
	///     Transaction is necessary in order to do a rollback after multiple saves in case an error is encountered
	/// </summary>
	/// <returns></returns>
	/// <exception cref="Exception"></exception>
	public async Task StartTransaction()
	{
		_transactionCount += 1;
		if (_transaction == null)
			try
			{
				_transaction = await _alarmCTX.Database.BeginTransactionAsync();
			}
			catch (Exception e)
			{
				if (e is not InvalidOperationException) throw new Exception(e.Message, e);

				throw new Exception("An error happened when starting the transaction", e);
			}
	}

	public async Task CommitTransaction()
	{
		if (_transaction != null && _transactionCount == 1)
			try
			{
				await _transaction.CommitAsync();
				_transaction = null;
			}
			catch (Exception e)
			{
				_transaction?.Rollback();
				_transaction = null;
				throw new Exception("An error happened when commiting transaction", e);
			}

		_transactionCount -= 1;
	}

	public void Dispose()
	{
		_alarmCTX.Dispose();
	}
}