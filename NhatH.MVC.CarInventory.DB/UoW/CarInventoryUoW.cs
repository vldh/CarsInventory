using NhatH.MVC.CarInventory.DB.Miscellaneous;
using NhatH.MVC.CarInventory.DB.Repository;
using NhatH.MVC.CarInventory.Domain;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;

namespace NhatH.MVC.CarInventory.DB.UoW
{
    public class CarInventoryUoW: IDisposable, ICarInventoryUoW
    {
        private readonly IRepositoryProvider _repositoryProvider;
        private DbTransaction _transaction;
        public DbContext DbContext { get; set; }
        private bool _disposed;

        public IBaseRepository<Car> Car
        {
            get { return GetStandardRepo<Car>(); }
        }
        public IBaseRepository<User> User
        {
            get { return GetStandardRepo<User>(); }
        }
        public IBaseRepository<UserProfile> UserProfile
        {
            get { return GetStandardRepo<UserProfile>(); }
        }

        public IBaseRepository<Role> Role
        {
            get { return GetStandardRepo<Role>(); }
        }
        public IBaseRepository<RoleFunction> RoleFunction
        {
            get { return GetStandardRepo<RoleFunction>(); }
        }

        public CarInventoryUoW(IRepositoryProvider repositoryProvider, CarInventoryDBContext dbContext)
        {
            DbContext = dbContext;
            _repositoryProvider = repositoryProvider;
            _repositoryProvider.DbContext = dbContext;
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
                throw new ApplicationException(
                    "Cannot begin a new transaction while an existing transaction is still running. " +
                    "Please commit or rollback the existing transaction before starting a new one.");
            }
            OpenConnection();
            _transaction = ((IObjectContextAdapter)DbContext).ObjectContext.Connection.BeginTransaction(isolationLevel);
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
                ((IObjectContextAdapter)DbContext).ObjectContext.SaveChanges();
                _transaction.Commit();
                ReleaseCurrentTransaction();
            }
            catch
            {
                RollBackTransaction();
                throw;
            }
        }

        public void Commit()
        {
            try
            {
                DbContext.SaveChanges();
            }
            catch (DbEntityValidationException entityValidationException)
            {
                throw entityValidationException;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!disposing)
                return;

            if (_disposed)
                return;

            _disposed = true;
        }

        private void OpenConnection()
        {
            if (((IObjectContextAdapter)DbContext).ObjectContext.Connection.State != ConnectionState.Open)
            {
                ((IObjectContextAdapter)DbContext).ObjectContext.Connection.Open();
            }
        }

        private void ReleaseCurrentTransaction()
        {
            if (_transaction != null)
            {
                _transaction.Dispose();
                _transaction = null;
            }
        }

        private IBaseRepository<T> GetStandardRepo<T>() where T : class
        {
            return _repositoryProvider.GetRepositoryForEntityType<T>();
        }

        private T GetSepcializedRepo<T>() where T : class
        {
            return _repositoryProvider.GetRepository<T>();
        }

        public IEnumerable<T> ExecuteRawSql<T>(string sql)
        {
            var carInventoryContext = DbContext as CarInventoryDBContext;
            if (DbContext != null)
            {
                var timeout = 1000 * 60 * 10;
                carInventoryContext.Database.CommandTimeout = timeout;
                return new List<T>(carInventoryContext.Database.SqlQuery<T>(sql));
            }
            return null;
        }

        public IBaseRepository<T> Repository<T>() where T : class
        {
            return GetStandardRepo<T>();
        }
    }
}
