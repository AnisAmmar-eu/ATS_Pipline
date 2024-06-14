using Core.Entities.Alarms.AlarmsC.Repositories;
using Core.Entities.Alarms.AlarmsCycle.Models.Repositories;
using Core.Entities.Alarms.AlarmsLog.Repositories;
using Core.Entities.Alarms.AlarmsRT.Repositories;
using Core.Entities.Anodes.Repositories;
using Core.Entities.BenchmarkTests.Repositories;
using Core.Entities.BenchmarkTests.Repositories.CameraTests;
using Core.Entities.BI.BITemperatures.Repositories;
using Core.Entities.DebugsModes.Repositories;
using Core.Entities.IOT.IOTDevices.Repositories;
using Core.Entities.IOT.IOTTags.Models.DB;
using Core.Entities.IOT.IOTTags.Repositories;
using Core.Entities.KPIData.KPIs.Repositories;
using Core.Entities.Packets.Repositories;
using Core.Entities.StationCycles.Repositories;
using Core.Entities.User.Repositories.Acts;
using Core.Entities.User.Repositories.Acts.ActEntities;
using Core.Entities.Vision.Testing.Repositories;
using Core.Entities.Vision.ToDos.Repositories.Datasets;
using Core.Entities.Vision.ToDos.Repositories.ToLoads;
using Core.Entities.Vision.ToDos.Repositories.ToMatchs;
using Core.Entities.Vision.ToDos.Repositories.ToNotifys;
using Core.Entities.Vision.ToDos.Repositories.ToSigns;
using Core.Entities.Vision.ToDos.Repositories.ToUnloads;
using Core.Shared.Data;
using Core.Shared.Repositories.System.Logs;
using Core.Shared.UnitOfWork.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;

namespace Core.Shared.UnitOfWork;

public class AnodeUOW : IAnodeUOW
{
	private readonly AnodeCTX _anodeCTX;
	private IDbContextTransaction? _transaction;
	private int _transactionCount;

	public ICameraTestRepository CameraTest { get; }
	public IBenchmarkTestRepository BenchmarkTest { get; }

	public ILogRepository Log { get; }
	public ILogEntryRepository Logs { get; }

	public IAlarmCRepository AlarmC { get; }
	public IAlarmLogRepository AlarmLog { get; }
	public IAlarmRTRepository AlarmRT { get; }

	public IAnodeRepository Anode { get; }

	public IPacketRepository Packet { get; }
	public IAlarmCycleRepository AlarmCycle { get; }

	public IStationCycleRepository StationCycle { get; }

	public IBITemperatureRepository BITemperature { get; }
	public IKPIRepository KPI { get; }

	public IIOTDeviceRepository IOTDevice { get; }
	public IMatchRepository Match { get; }
	public ISignRepository Sign { get; }

	public IIOTTagRepository IOTTag { get; }

	public IToMatchRepository ToMatch { get; }
	public IToLoadRepository ToLoad { get; }
	public IToSignRepository ToSign { get; }
	public IToUnloadRepository ToUnload { get; }
	public IToNotifyRepository ToNotify { get; }
	public IDatasetRepository Dataset { get; }
	public IDebugModeRepository DebugMode { get; }

	public IStationTestRepository StationTest { get; }

	public AnodeUOW(AnodeCTX anodeCTX)
	{
		_anodeCTX = anodeCTX;

		BenchmarkTest = new BenchmarkTestRepository(_anodeCTX);
		CameraTest = new CameraTestRepository(_anodeCTX);

		Log = new LogRepository(_anodeCTX);
		Logs = new LogEntryRepository(_anodeCTX);

		AlarmC = new AlarmCRepository(_anodeCTX);
		AlarmLog = new AlarmLogRepository(_anodeCTX);
		AlarmRT = new AlarmRTRepository(_anodeCTX);

		Anode = new AnodeRepository(_anodeCTX);

		Packet = new PacketRepository(_anodeCTX);
		AlarmCycle = new AlarmCycleRepository(_anodeCTX);

		StationCycle = new StationCycleRepository(_anodeCTX);

		BITemperature = new BITemperatureRepository(_anodeCTX);
		KPI = new KPIRepository(_anodeCTX);

		IOTDevice = new IOTDeviceRepository(_anodeCTX);
		Match = new MatchRepository(_anodeCTX);
		Sign = new SignRepository(_anodeCTX);

		IOTTag = new IOTTagRepository(_anodeCTX);

		Acts = new ActRepository(_anodeCTX);
		ActEntities = new ActEntityRepository(_anodeCTX);

		ToMatch = new ToMatchRepository(_anodeCTX);
		ToLoad = new ToLoadRepository(_anodeCTX);
		ToSign = new ToSignRepository(_anodeCTX);
		ToUnload = new ToUnloadRepository(_anodeCTX);
		ToNotify = new ToNotifyRepository(_anodeCTX);
		Dataset = new DatasetRepository(_anodeCTX);

		StationTest = new StationTestRepository(_anodeCTX);
		DebugMode = new DebugModeRepository(_anodeCTX);
	}

	public object? GetRepoByType(Type repo)
	{
		return repo switch {
			_ when repo == typeof(IBenchmarkTestRepository) => BenchmarkTest,

			_ when repo == typeof(IAlarmCRepository) => AlarmC,
			_ when repo == typeof(IAlarmLogRepository) => AlarmLog,
			_ when repo == typeof(IAlarmRTRepository) => AlarmRT,

			_ when repo == typeof(IAnodeRepository) => Anode,

			_ when repo == typeof(IPacketRepository) => Packet,
			_ when repo == typeof(IAlarmCycleRepository) => Packet,

			_ when repo == typeof(IStationCycleRepository) => StationCycle,

			_ when repo == typeof(IBITemperatureRepository) => BITemperature,
			_ when repo == typeof(IKPIRepository) => KPI,

			_ when repo == typeof(IIOTDeviceRepository) => IOTDevice,
			_ when repo == typeof(IMatchRepository) => Match,
			_ when repo == typeof(ISignRepository) => Sign,

			_ when repo == typeof(IIOTTagRepository) => IOTTag,

			_ when repo == typeof(IActRepository) => Acts,
			_ when repo == typeof(IActEntityRepository) => ActEntities,

			_ when repo == typeof(IToSignRepository) => ToSign,
			_ when repo == typeof(IToMatchRepository) => ToMatch,
			_ when repo == typeof(IToLoadRepository) => ToLoad,
			_ when repo == typeof(IToSignRepository) => ToSign,
			_ when repo == typeof(IToUnloadRepository) => ToUnload,
			_ when repo == typeof(IToNotifyRepository) => ToNotify,
			_ when repo == typeof(IDatasetRepository) => Dataset,
			_ when repo == typeof(IDebugModeRepository) => DebugMode,

			_ when repo == typeof(IStationTestRepository) => StationTest,

			_ when repo == typeof(ILogRepository) => Log,
			_ when repo == typeof(ILogEntryRepository) => Logs,
			_ => null,
		};
	}

	public int Commit(bool isNewValue = false)
	{
		// There is a while true bc in case of concurrency exception, we have to retry SaveChangesAsync().
		while (true)
		{
			try
			{
				// return _anodeCTX.SaveChangesAsync().Result;
				return _anodeCTX.SaveChanges();
			}
			catch (DbUpdateConcurrencyException e)
			{
				foreach (EntityEntry entry in e.Entries)
				{
					if (entry.Entity is IOTTag)
					{
						PropertyValues proposedValues = entry.CurrentValues;
						PropertyValues databaseValues = entry.GetDatabaseValues()!;

						if (isNewValue)
						{
							proposedValues["CurrentValue"] = databaseValues["CurrentValue"];
						}
						else
						{
							proposedValues["NewValue"] = databaseValues["NewValue"];
							proposedValues["HasNewValue"] = databaseValues["HasNewValue"];
						}

						entry.OriginalValues.SetValues(databaseValues);
					}
					else
					{
						throw;
					}
				}
			}
			catch (Exception e)
			{
				if (_transaction is not null)
				{
					_transaction.Rollback();
					_transaction = null;
				}

				throw new("An error happened during SaveChanges", e);
			}
		}
	}

	/// <summary>
	///     Transaction is necessary in order to do a rollback after multiple saves in case an error is encountered
	/// </summary>
	/// <exception cref="Exception"></exception>
	public async Task StartTransaction()
	{
		_transactionCount += 1;
		if (_transaction is not null)
			return;

		try
		{
			_transaction = await _anodeCTX.Database.BeginTransactionAsync();
		}
		catch (Exception e)
		{
			if (e is not InvalidOperationException)
				throw new(e.Message, e);

			throw new("An error happened when starting the transaction", e);
		}
	}

	public async Task CommitTransaction()
	{
		if (_transaction is not null && _transactionCount == 1)
		{
			try
			{
				await _transaction.CommitAsync();
				_transaction = null;
			}
			catch (Exception e)
			{
				_transaction?.Rollback();
				_transaction = null;
				throw new("An error happened when commiting transaction", e);
			}
		}

		_transactionCount -= 1;
	}

	public void Dispose()
	{
		_anodeCTX.Dispose();
		GC.SuppressFinalize(this);
	}

	public int GetTransactionCount() => _transactionCount;

	public bool GetTransactionIsNull() => _transaction is null;

	#region Users

	public IActRepository Acts { get; }
	public IActEntityRepository ActEntities { get; }

	#endregion Users
}