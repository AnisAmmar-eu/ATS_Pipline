using Core.Entities.Alarms.AlarmsC.Repositories;
using Core.Entities.Alarms.AlarmsCycle.Models.Repositories;
using Core.Entities.Alarms.AlarmsLog.Repositories;
using Core.Entities.Alarms.AlarmsPLC.Repositories;
using Core.Entities.Alarms.AlarmsRT.Repositories;
using Core.Entities.ExtTags.Repositories;
using Core.Entities.KPI.KPICs.Repositories;
using Core.Entities.KPI.KPIEntries.Repositories;
using Core.Entities.KPI.KPIEntries.Repositories.KPILogs;
using Core.Entities.KPI.KPIEntries.Repositories.KPIRTs;
using Core.Entities.Packets.Repositories;
using Core.Entities.ServicesMonitors.Repositories;
using Core.Entities.User.Repositories.Acts;
using Core.Entities.User.Repositories.Acts.ActEntities;
using Core.Entities.User.Repositories.Roles;
using Core.Shared.Data;
using Core.Shared.Repositories.System.Logs;
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

		Log = new LogRepository(_alarmCTX);

		AlarmC = new AlarmCRepository(_alarmCTX);
		AlarmPLC = new AlarmPLCRepository(_alarmCTX);
		AlarmLog = new AlarmLogRepository(_alarmCTX);
		AlarmRT = new AlarmRTRepository(_alarmCTX);

		Packet = new PacketRepository(_alarmCTX);
		AlarmCycle = new AlarmCycleRepository(_alarmCTX);

		KPIC = new KPICRepository(_alarmCTX);
		KPILog = new KPILogRepository(_alarmCTX);
		KPIRT = new KPIRTRepository(_alarmCTX);

		ExtTag = new ExtTagRepository(_alarmCTX);
		ServicesMonitor = new ServicesMonitorRepository(_alarmCTX);

		Acts = new ActRepository(_alarmCTX);
		ActEntities = new ActEntityRepository(_alarmCTX);
		Roles = new RoleRepository(_alarmCTX);
	}

	public ILogRepository Log { get; }

	public IAlarmCRepository AlarmC { get; }
	public IAlarmPLCRepository AlarmPLC { get; }
	public IAlarmLogRepository AlarmLog { get; }
	public IAlarmRTRepository AlarmRT { get; }

	public IPacketRepository Packet { get; }
	public IAlarmCycleRepository AlarmCycle { get; }
	
	// KPI
	public IKPICRepository KPIC { get; }
	public IKPILogRepository KPILog { get; }
	public IKPIRTRepository KPIRT { get; }

	public IExtTagRepository ExtTag { get; }
	public IServicesMonitorRepository ServicesMonitor { get; }

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
			
			_ when repo == typeof(IKPICRepository) => KPIC,
			_ when repo == typeof(IKPILogRepository) => KPILog,
			_ when repo == typeof(IKPIRTRepository) => KPIRT,

			_ when repo == typeof(IExtTagRepository) => ExtTag,
			_ when repo == typeof(IServicesMonitorRepository) => ServicesMonitor,

			_ when repo == typeof(IActRepository) => Acts,
			_ when repo == typeof(IActEntityRepository) => ActEntities,
			_ when repo == typeof(IRoleRepository) => Roles,
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

	#region Users

	public IActRepository Acts { get; }
	public IActEntityRepository ActEntities { get; }
	public IRoleRepository Roles { get; }

	#endregion
}