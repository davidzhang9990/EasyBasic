using System;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using Common.EntityFramework.Config;
using Common.EntityFramework.DataAccess;

namespace Common.EntityFramework.DataAccess
{
    internal class UnitOfWork : IUnitOfWork
    {
        private DbContextTransaction _transaction;
        private readonly DbContext _dbContext;

        public UnitOfWork(DbContext context)
        {
            _dbContext = context;
        }

        public bool IsInTransaction
        {
            get { return _transaction != null; }
        }

        public void BeginTransaction()
        {
            BeginTransaction(IsolationLevel.ReadCommitted);
        }

        public void BeginTransaction(IsolationLevel isolationLevel)
        {
            if (_transaction != null)
            {
                throw new ApplicationException("Cannot begin a new transaction while an existing transaction is still running. " +
                                                "Please commit or rollback the existing transaction before starting a new one.");
            }
            OpenConnection();
            _transaction = _dbContext.Database.BeginTransaction(isolationLevel);
            //_transaction = ((IObjectContextAdapter)_dbContext).ObjectContext.Connection.BeginTransaction(isolationLevel);
        }

        public void RollBackTransaction()
        {
            if (_transaction == null)
            {
                throw new ApplicationException("Cannot roll back a transaction while there is no transaction running.");
            }

            if (IsInTransaction)
            {
                _transaction.Rollback();
                ReleaseCurrentTransaction();
            }
        }

        public void CommitTransaction()
        {
            if (_transaction == null)
            {
                throw new ApplicationException("Cannot roll back a transaction while there is no transaction running.");
            }

            try
            {
                _dbContext.SaveChanges();
                _transaction.Commit();
                ReleaseCurrentTransaction();
            }
            catch (DbEntityValidationException e)
            {
                RollBackTransaction();
                foreach (var eve in e.EntityValidationErrors)
                {
                    DbLogFormatter.Logger.ErrorFormat("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        DbLogFormatter.Logger.ErrorFormat("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }
            catch
            {
                RollBackTransaction();
                throw;
            }
        }

        public void SaveChanges()
        {
            if (IsInTransaction)
            {
                throw new ApplicationException("A transaction is running. Call CommitTransaction instead.");
            }
            _dbContext.SaveChanges();
        }


        #region Implementation of IDisposable

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposes off the managed and unmanaged resources used.
        /// </summary>
        /// <param name="disposing"></param>
        private void Dispose(bool disposing)
        {
            if (!disposing)
                return;

            if (_disposed)
                return;

            _disposed = true;
        }

        private bool _disposed;
        #endregion

        private void OpenConnection()
        {
            if (_dbContext.Database.Connection.State != ConnectionState.Open)
            {
                _dbContext.Database.Connection.Open();
            }
        }

        /// <summary>
        /// Releases the current transaction
        /// </summary>
        private void ReleaseCurrentTransaction()
        {
            if (_transaction != null)
            {
                _transaction.Dispose();
                _transaction = null;
            }
        }
    }
}
