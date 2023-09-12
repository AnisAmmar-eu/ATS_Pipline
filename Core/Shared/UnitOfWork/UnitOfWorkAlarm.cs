using Core.Entities.Alarmes_C.Repositories;
using Core.Entities.AlarmesPLC.Repositories;
using Core.Entities.Journals.Models.DB;
using Core.Entities.Journals.Repositories;
using Core.Shared.Data;
using Core.Shared.Repositories.Kernel;
using Core.Shared.Repositories.Kernel.Interfaces;
using Core.Shared.UnitOfWork.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;

namespace Core.Shared.UnitOfWork
{
    public class UnitOfWorkAlarm : IUnitOfWorkAlarm
    {
        private readonly AlarmesDbContext _context;
        private IDbContextTransaction? _transaction;
        private int _transactionCount = 0;

        public UnitOfWorkAlarm(AlarmesDbContext context)
        {
            _context = context;
            
            Alarme_C = new Alarme_CRepository(_context);
            AlarmePLC = new AlarmePLCRepository(_context);
            Journal = new JournalRepository(_context);
        }


        public IAlarme_CRepository Alarme_C { get; }
        public IAlarmePLCRepository AlarmePLC { get; }
        public IJournalRepository Journal { get; }

        public int Commit()
        {
            try
            {
                return _context.SaveChangesAsync().Result;
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
        /// Transaction is necessary in order to do a rollback after multiple saves in case an error is encountered
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task StartTransaction()
        {
            _transactionCount += 1;
            if (_transaction == null)
            {
                try
                {
                    _transaction = await _context.Database.BeginTransactionAsync();
                }
                catch (Exception e)
                {
                    if (e is not InvalidOperationException)
                    {
                        throw new Exception(e.Message, e);
                    }

                    throw new Exception("An error happened when starting the transaction", e);
                }
            }
        }

        public async Task CommitTransaction()
        {
            if (_transaction != null && _transactionCount == 1)
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
                    throw new Exception("An error happened when commiting transaction", e);
                }
            }

            _transactionCount -= 1;
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}