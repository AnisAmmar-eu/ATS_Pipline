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
using Core.Shared.Repositories.System.Logs;

namespace Core.Shared.UnitOfWork.Interfaces;

/// <summary>
///     Interface IUnitOfWork defines the methods that are required to be implemented by a Unit of Work class.
/// </summary>
public interface IAlarmUOW : IDisposable
{
	ILogRepository Log { get; }
	IAlarmCRepository AlarmC { get; }
	IAlarmPLCRepository AlarmPLC { get; }
	IAlarmLogRepository AlarmLog { get; }
	IAlarmRTRepository AlarmRT { get; }

	// KPI
	IKPICRepository KPIC { get; }
	IKPILogRepository KPILog { get; }
	IKPIRTRepository KPIRT { get; }
	
	IPacketRepository Packet { get; }
	IAlarmCycleRepository AlarmCycle { get; }

	IExtTagRepository ExtTag { get; }
	IServicesMonitorRepository ServicesMonitor { get; }

	public object? GetRepoByType(Type repo);

	/// <summary>
	///     Saves changes made in this context to the underlying database.
	/// </summary>
	/// <returns></returns>
	int Commit();

	Task StartTransaction();

	Task CommitTransaction();

	#region Users

	public IActRepository Acts { get; }
	public IActEntityRepository ActEntities { get; }
	public IRoleRepository Roles { get; }

	#endregion
}