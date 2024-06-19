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
using Core.Shared.Repositories.System.Logs;

namespace Core.Shared.UnitOfWork.Interfaces;

/// <summary>
///     Interface IUnitOfWork defines the methods that are required to be implemented by a Unit of Work class.
/// </summary>
public interface IAnodeUOW : IDisposable
{
	IBenchmarkTestRepository BenchmarkTest { get; }
	ICameraTestRepository CameraTest { get; }

	ILogRepository Logs { get; }

	IAlarmCRepository AlarmC { get; }
	IAlarmLogRepository AlarmLog { get; }
	IAlarmRTRepository AlarmRT { get; }

	IAnodeRepository Anode { get; }

	IStationCycleRepository StationCycle { get; }

	IBITemperatureRepository BITemperature { get; }
	IKPIRepository KPI { get; }

	IIOTDeviceRepository IOTDevice { get; }
	IMatchRepository Match { get; }
	ISignRepository Sign { get; }

	IIOTTagRepository IOTTag { get; }

	IPacketRepository Packet { get; }
	IAlarmCycleRepository AlarmCycle { get; }

	IToMatchRepository ToMatch { get; }
	IToLoadRepository ToLoad { get; }
	IToNotifyRepository ToNotify { get; }
	IToSignRepository ToSign { get; }
	IToUnloadRepository ToUnload { get; }
	IDatasetRepository Dataset { get; }
	IDebugModeRepository DebugMode { get; }

	IStationTestRepository StationTest { get; }

	public object? GetRepoByType(Type repo);

	/// <summary>
	///     Saves changes made in this context to the underlying database.
	/// </summary>
	/// <param name="isNewValue"></param>
	int Commit(bool isNewValue = false);

	Task StartTransaction();

	Task CommitTransaction();

	int GetTransactionCount();

	bool GetTransactionIsNull();

	#region Users

	public IActRepository Acts { get; }
	public IActEntityRepository ActEntities { get; }

	#endregion Users
}