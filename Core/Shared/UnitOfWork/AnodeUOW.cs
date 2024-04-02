using Core.Entities.Alarms.AlarmsC.Repositories;
using Core.Entities.Alarms.AlarmsCycle.Models.Repositories;
using Core.Entities.Alarms.AlarmsLog.Repositories;
using Core.Entities.Alarms.AlarmsRT.Repositories;
using Core.Entities.Anodes.Repositories;
using Core.Entities.BenchmarkTests.Repositories;
using Core.Entities.BenchmarkTests.Repositories.CameraTests;
using Core.Entities.BI.BITemperatures.Repositories;
using Core.Entities.IOT.IOTDevices.Repositories;
using Core.Entities.IOT.IOTTags.Models.DB;
using Core.Entities.IOT.IOTTags.Repositories;
using Core.Entities.KPI.KPICs.Repositories;
using Core.Entities.KPI.KPIEntries.Repositories.KPILogs;
using Core.Entities.KPI.KPIEntries.Repositories.KPIRTs;
using Core.Entities.Packets.Repositories;
using Core.Entities.StationCycles.Repositories;
using Core.Entities.User.Repositories.Acts;
using Core.Entities.User.Repositories.Acts.ActEntities;
using Core.Entities.User.Repositories.Roles;
using Core.Entities.Vision.FileSettings.Repositories;
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

	public IAlarmCRepository AlarmC { get; }
	public IAlarmLogRepository AlarmLog { get; }
	public IAlarmRTRepository AlarmRT { get; }

	public IAnodeRepository Anode { get; }

	public IPacketRepository Packet { get; }
	public IAlarmCycleRepository AlarmCycle { get; }

	public IStationCycleRepository StationCycle { get; }

	public IKPICRepository KPIC { get; }
	public IKPILogRepository KPILog { get; }
	public IKPIRTRepository KPIRT { get; }
	public IBITemperatureRepository BITemperature { get; }

	public IIOTDeviceRepository IOTDevice { get; }
	public IIOTTagRepository IOTTag { get; }

	public IFileSettingRepository FileSetting { get; }
	public IToMatchRepository ToMatch { get; }
	public IToLoadRepository ToLoad { get; }
	public IToSignRepository ToSign { get; }
	public IToUnloadRepository ToUnload { get; }
	public IToNotifyRepository ToNotify { get; }
	public IDatasetRepository Dataset { get; }

	public AnodeUOW(AnodeCTX anodeCTX)
	{
		_anodeCTX = anodeCTX;

		BenchmarkTest = new BenchmarkTestRepository(_anodeCTX);
		CameraTest = new CameraTestRepository(_anodeCTX);

		Log = new LogRepository(_anodeCTX);

		AlarmC = new AlarmCRepository(_anodeCTX);
		AlarmLog = new AlarmLogRepository(_anodeCTX);
		AlarmRT = new AlarmRTRepository(_anodeCTX);

		Anode = new AnodeRepository(_anodeCTX);

		Packet = new PacketRepository(_anodeCTX);
		AlarmCycle = new AlarmCycleRepository(_anodeCTX);

		StationCycle = new StationCycleRepository(_anodeCTX);

		KPIC = new KPICRepository(_anodeCTX);
		KPILog = new KPILogRepository(_anodeCTX);
		KPIRT = new KPIRTRepository(_anodeCTX);
		BITemperature = new BITemperatureRepository(_anodeCTX);

		IOTDevice = new IOTDeviceRepository(_anodeCTX);
		IOTTag = new IOTTagRepository(_anodeCTX);

		Acts = new ActRepository(_anodeCTX);
		ActEntities = new ActEntityRepository(_anodeCTX);
		Roles = new RoleRepository();

		FileSetting = new FileSettingRepository(_anodeCTX);
		ToMatch = new ToMatchRepository(_anodeCTX);
		ToLoad = new ToLoadRepository(_anodeCTX);
		ToSign = new ToSignRepository(_anodeCTX);
		ToUnload = new ToUnloadRepository(_anodeCTX);
		ToNotify = new ToNotifyRepository(_anodeCTX);
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

			_ when repo == typeof(IKPICRepository) => KPIC,
			_ when repo == typeof(IKPILogRepository) => KPILog,
			_ when repo == typeof(IKPIRTRepository) => KPIRT,
			_ when repo == typeof(IBITemperatureRepository) => BITemperature,

			_ when repo == typeof(IIOTDeviceRepository) => IOTDevice,
			_ when repo == typeof(IIOTTagRepository) => IOTTag,

			_ when repo == typeof(IActRepository) => Acts,
			_ when repo == typeof(IActEntityRepository) => ActEntities,
			_ when repo == typeof(IRoleRepository) => Roles,

			_ when repo == typeof(IFileSettingRepository) => FileSetting,
			_ when repo == typeof(IToSignRepository) => ToSign,
			_ when repo == typeof(IToMatchRepository) => ToMatch,
            _ when repo == typeof(IToLoadRepository) => ToLoad,
			_ when repo == typeof(IToSignRepository) => ToSign,
			_ when repo == typeof(IToUnloadRepository) => ToUnload,
			_ when repo == typeof(IToNotifyRepository) => ToNotify,

			_ when repo == typeof(ILogRepository) => Log,
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

	public int GetTransactionCount()
	{
		return _transactionCount;
	}

	public bool GetTransactionIsNull()
	{
		return _transaction is null;
	}

	#region Users

	public IActRepository Acts { get; }
	public IActEntityRepository ActEntities { get; }
	public IRoleRepository Roles { get; }

	#endregion
}