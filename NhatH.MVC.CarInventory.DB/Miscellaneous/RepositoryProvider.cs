using NhatH.MVC.CarInventory.DB.Repository;
using System;
using System.Collections.Generic;
using System.Data.Entity;

namespace NhatH.MVC.CarInventory.DB.Miscellaneous
{
    public class RepositoryProvider : IRepositoryProvider
    {
        private readonly IRepositoryFactories _repositoryFactories;
        protected Dictionary<Type, object> Repositories { get; private set; }

        public DbContext DbContext { get; set; }

        public RepositoryProvider(IRepositoryFactories repositoryFactories)
        {
            _repositoryFactories = repositoryFactories;
            Repositories = new Dictionary<Type, object>();
        }

        public IBaseRepository<T> GetRepositoryForEntityType<T>() where T : class
        {
            return GetRepository<IBaseRepository<T>>(
                _repositoryFactories.GetRepositoryFactoryForEntityType<T>());
        }

        public virtual T GetRepository<T>(Func<DbContext, object> factory = null) where T : class
        {
            object repoObj;
            Repositories.TryGetValue(typeof(T), out repoObj);
            if (repoObj != null)
            {
                return (T)repoObj;
            }

            // Not found or null; make one, add to dictionary cache, and return it.
            return MakeRepository<T>(factory, DbContext);
        }

        protected virtual T MakeRepository<T>(Func<DbContext, object> factory, DbContext dbContext)
        {
            Func<DbContext, object> f = factory ?? _repositoryFactories.GetRepositoryFactory<T>();
            if (f == null)
            {
                throw new NotImplementedException("No factory for repository type, " + typeof(T).FullName);
            }
            var repo = (T)f(dbContext);
            Repositories[typeof(T)] = repo;
            return repo;
        }

        public void SetRepository<T>(T repository)
        {
            Repositories[typeof(T)] = repository;
        }
    }
}
