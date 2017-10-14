using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace NhatH.MVC.CarInventory.DB.Repository
{
    public interface IBaseRepository<T> where T : class
    {
        void Attach(T entity);
        void Detach(T entity);
        T GetById(object id);
        void Insert(T entity);
        void Update(T entity);
        void Delete(T entity);
        void Delete(IEnumerable<T> entity);
        void Delete(object id);

        T FindSingleOrDefault(Expression<Func<T, bool>> criteria, string[] includes = null);
        T FindFirstOfDefault(Expression<Func<T, bool>> criteria, string[] includes = null);
        T FindFirstOfDefault(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes);

        IQueryable<T> GetAll();
        IQueryable<T> Find(Expression<Func<T, bool>> criteria, string[] includes = null);
        IQueryable<T> Find<TKey>(Expression<Func<T, bool>> criteria, Expression<Func<T, TKey>> sort,
                                 bool desc = false, string[] includes = null);

        int Count(Expression<Func<T, bool>> predicate);
        bool Any(Expression<Func<T, bool>> predicate);
        void ExecuteRawSql(string sql);
        T GetOneFromSql(string sql, params object[] parameters);
        IEnumerable<T> GetManyFromSql(string sql, params object[] parameters);
        DataTable AsDataTable();

        void Refresh(T instance);
        T GetLatestById(object id);
    }
}
