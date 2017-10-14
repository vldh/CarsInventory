using NhatH.MVC.CarInventory.DB.Repository;
using NhatH.MVC.CarInventory.Domain;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NhatH.MVC.CarInventory.DB.UoW
{
    public interface ICarInventoryUoW
    {
        DbContext DbContext { get; set; }

        IBaseRepository<Car> Car { get; }
        IBaseRepository<User> User { get; }
        IBaseRepository<UserProfile> UserProfile { get; }
        IBaseRepository<Role> Role { get; }
        IBaseRepository<RoleFunction> RoleFunction { get; }
        IBaseRepository<T> Repository<T>() where T : class;
        bool IsInTransaction { get; }
        void BeginTransaction();
        void BeginTransaction(IsolationLevel isolationLevel);
        void RollBackTransaction();
        void CommitTransaction();
        void Commit();
        void Dispose();
    }
}
