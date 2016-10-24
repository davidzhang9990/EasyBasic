using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.EntityFramework.DataAccess
{
    public interface IUnitOfWork : IDisposable
    {
        //ITokenExtractor TokenExtractor { get; set; }
        /// <summary>
        /// Gets a value indicating whether is in transaction.
        /// </summary>
        bool IsInTransaction { get; }

        /// <summary>
        /// The save changes.
        /// </summary>
        void SaveChanges();

        /// <summary>
        /// The begin transaction.
        /// </summary>
        void BeginTransaction();

        /// <summary>
        /// The begin transaction.
        /// </summary>
        /// <param name="isolationLevel">
        /// The isolation level.
        /// </param>
        void BeginTransaction(IsolationLevel isolationLevel);

        /// <summary>
        /// The roll back transaction.
        /// </summary>
        void RollBackTransaction();

        /// <summary>
        /// The commit transaction.
        /// </summary>
        void CommitTransaction();
    }
}
