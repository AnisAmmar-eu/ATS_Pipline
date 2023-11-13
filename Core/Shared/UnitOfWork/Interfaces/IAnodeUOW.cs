using Core.Entities.Alarms.AlarmsC.Repositories;
using Core.Entities.Alarms.AlarmsCycle.Models.Repositories;
using Core.Entities.Alarms.AlarmsLog.Repositories;
using Core.Entities.Alarms.AlarmsPLC.Repositories;
using Core.Entities.Alarms.AlarmsRT.Repositories;
using Core.Entities.Anodes.Repositories;
using Core.Entities.BI.BITemperatures.Repositories;
using Core.Entities.IOT.IOTDevices.Repositories;
using Core.Entities.IOT.IOTTags.Repositories;
using Core.Entities.KPI.KPICs.Repositories;
using Core.Entities.KPI.KPIEntries.Repositories.KPILogs;
using Core.Entities.KPI.KPIEntries.Repositories.KPIRTs;
using Core.Entities.KPI.KPITests.Repositories;
using Core.Entities.Packets.Repositories;
using Core.Entities.StationCycles.Repositories;
using Core.Entities.User.Repositories.Acts;
using Core.Entities.User.Repositories.Acts.ActEntities;
using Core.Entities.User.Repositories.Roles;
using Core.Entities.Vision.FileSettings.Repositories;
using Core.Shared.Repositories.System.Logs;

namespace Core.Shared.UnitOfWork.Interfaces;

/// <summary>
///     Interface IUnitOfWork defines the methods that are required to be implemented by a Unit of Work class.
/// </summary>
public interface IAnodeUOW : IDisposable
{
	ILogRepository Log { get; }
	IAlarmCRepository AlarmC { get; }
	IAlarmPLCRepository AlarmPLC { get; }
	IAlarmLogRepository AlarmLog { get; }
	IAlarmRTRepository AlarmRT { get; }

	IAnodeRepository Anode { get; }

	// StationCycle
	IStationCycleRepository StationCycle { get; }

	// KPI
	IKPICRepository KPIC { get; }
	IKPILogRepository KPILog { get; }
	IKPIRTRepository KPIRT { get; }
	IKPITestRepository KPITest { get; }
	IBITemperatureRepository BITemperature { get; }

	// IOT
	IIOTDeviceRepository IOTDevice { get; }
	IIOTTagRepository IOTTag { get; }

	IPacketRepository Packet { get; }
	IAlarmCycleRepository AlarmCycle { get; }

	// Vision
	IFileSettingRepository FileSetting { get; }

	public object? GetRepoByType(Type repo);

	/// <summary>
	///     Saves changes made in this context to the underlying database.
	/// </summary>
	/// <returns></returns>
	int Commit(bool isNewValue = false);

	Task StartTransaction();

	Task CommitTransaction();

	#region Users

	public IActRepository Acts { get; }
	public IActEntityRepository ActEntities { get; }
	public IRoleRepository Roles { get; }

	#endregion
}