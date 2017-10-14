using System;
using System.Data.Entity;

namespace NhatH.MVC.CarInventory.DB.Miscellaneous
{
    public interface IRepositoryFactories
    {
        Func<DbContext, object> GetRepositoryFactory<T>();
        Func<DbContext, object> GetRepositoryFactoryForEntityType<T>() where T : class;
    }
}
