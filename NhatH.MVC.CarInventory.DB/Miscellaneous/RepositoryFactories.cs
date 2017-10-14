using NhatH.MVC.CarInventory.DB.Repository;
using System;
using System.Collections.Generic;
using System.Data.Entity;

namespace NhatH.MVC.CarInventory.DB.Miscellaneous
{
    public class RepositoryFactories : IRepositoryFactories
    {
        private readonly IDictionary<Type, Func<DbContext, object>> _repositoryFactories;

        public RepositoryFactories()
        {
            _repositoryFactories = GetFactories();
        }

        private IDictionary<Type, Func<DbContext, object>> GetFactories()
        {
            return new Dictionary<Type, Func<DbContext, object>>
            {
                // TODO. Add specialized repository here.
            };
        }

        public Func<DbContext, object> GetRepositoryFactory<T>()
        {
            Func<DbContext, object> factory;
            _repositoryFactories.TryGetValue(typeof(T), out factory);
            return factory;
        }

        public Func<DbContext, object> GetRepositoryFactoryForEntityType<T>() where T : class
        {
            return GetRepositoryFactory<T>() ?? DefaultEntityRepositoryFactory<T>();
        }

        protected virtual Func<DbContext, object> DefaultEntityRepositoryFactory<T>() where T : class
        {
            return dbContext => new BaseRepository<T>(dbContext);
        }
    }
}
