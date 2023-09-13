using Core.Entities.AlarmsC.Repositories;
using Core.Entities.AlarmsPLC.Repositories;
using Core.Entities.Journals.Repositories;

namespace Core.Shared.UnitOfWork.Interfaces;

/// <summary>
///     Interface IUnitOfWork defines the methods that are required to be implemented by a Unit of Work class.
/// </summary>
public interface IAlarmUOW : IDisposable
{
	IAlarmCRepository AlarmC { get; }
	IAlarmPLCRepository AlarmPLC { get; }
	IJournalRepository Journal { get; }

	/// <summary>
	///     Saves changes made in this context to the underlying database.
	/// </summary>
	/// <returns></returns>
	int Commit();

	Task StartTransaction();

	Task CommitTransaction();
}