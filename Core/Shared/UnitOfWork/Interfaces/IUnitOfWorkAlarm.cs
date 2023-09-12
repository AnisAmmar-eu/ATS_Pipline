using Core.Entities.Alarmes_C.Repositories;
using Core.Entities.AlarmesPLC.Repositories;
using Core.Entities.Journals.Repositories;
using Core.Shared.Repositories.Kernel.Interfaces;

namespace Core.Shared.UnitOfWork.Interfaces
{
    /// <summary>
    ///     Interface IUnitOfWork defines the methods that are required to be implemented by a Unit of Work class.
    /// </summary>
    public interface IUnitOfWorkAlarm : IDisposable
    {

        IAlarme_CRepository Alarme_C { get; }
        IAlarmePLCRepository AlarmePLC { get; }
        IJournalRepository Journal { get; }

        /// <summary>
        ///     Saves changes made in this context to the underlying database.
        /// </summary>
        /// <returns></returns>
        int Commit();

        Task StartTransaction();

        Task CommitTransaction();
       
    }
}
