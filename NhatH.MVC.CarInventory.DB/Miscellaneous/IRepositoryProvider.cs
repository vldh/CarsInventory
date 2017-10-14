using NhatH.MVC.CarInventory.DB.Repository;
using System;
using System.Data.Entity;

namespace NhatH.MVC.CarInventory.DB.Miscellaneous
{
    public interface IRepositoryProvider
    {

        DbContext DbContext { get; set; }
        IBaseRepository<T> GetRepositoryForEntityType<T>() where T : class;
        T GetRepository<T>(Func<DbContext, object> factory = null) where T : class;
        void SetRepository<T>(T repository);
    }
}
