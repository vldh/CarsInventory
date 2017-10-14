using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace NhatH.MVC.CarInventory.DB.Repository
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        protected DbContext DbContext { get; set; }
        protected IDbSet<T> DbSet { get; set; }

        public BaseRepository(DbContext dbContext)
        {
            if (dbContext == null)
                throw new ArgumentNullException("dbContext");

            DbContext = dbContext;
            DbSet = DbContext.Set<T>();
        }

        public void Attach(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }

            DbSet.Attach(entity);
        }

        public void Detach(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }

            DbEntityEntry dbEntityEntry = DbContext.Entry(entity);
            if (dbEntityEntry.State != System.Data.Entity.EntityState.Detached)
            {
                dbEntityEntry.State = System.Data.Entity.EntityState.Detached;
            }
        }

        public T GetById(object id)
        {
            return Entities.Find(id);
        }

        public T GetLatestById(object id)
        {
            DbContext.Entry<T>(GetById(id)).Reload();
            return GetById(id);
        }

        public void Insert(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }

            DbSet.Add(entity);
        }

        public void Update(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            DbEntityEntry dbEntityEntry = DbContext.Entry(entity);
            if (dbEntityEntry.State == System.Data.Entity.EntityState.Detached)
            {
                DbSet.Attach(entity);
            }
            dbEntityEntry.State = System.Data.Entity.EntityState.Modified;
        }

        public void Delete(IEnumerable<T> entities)
        {
            var temp = new List<T>(entities);
            foreach (var entity in temp)
            {
                Delete(entity);
            }
        }

        public void Delete(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            DbEntityEntry dbEntityEntry = DbContext.Entry(entity);
            if (dbEntityEntry.State != System.Data.Entity.EntityState.Deleted)
            {
                dbEntityEntry.State = System.Data.Entity.EntityState.Deleted;
            }
            else
            {
                DbSet.Attach(entity);
                DbSet.Remove(entity);
            }
        }

        public void Delete(object id)
        {
            Delete(GetById(id));
        }

        public T FindSingleOrDefault(Expression<Func<T, bool>> criteria, string[] includes = null)
        {
            if (criteria == null)
                throw new ArgumentNullException("criteria");

            if (includes == null) return DbSet.Where(criteria.Compile()).FirstOrDefault();

            foreach (string property in includes)
            {
                DbSet.Include(property);
            }

            return DbSet.Where(criteria.Compile()).FirstOrDefault();
        }

        public T FindFirstOfDefault(Expression<Func<T, bool>> criteria, string[] includes = null)
        {
            var compiledcriteria = criteria.Compile();
            if (criteria == null)
                throw new ArgumentNullException("criteria");
            if (includes != null)
            {
                foreach (string property in includes)
                {
                    DbSet.Include(property);
                }
            }

            var entity = DbSet.Local.FirstOrDefault(compiledcriteria);

            if (entity == null)
            {
                entity = DbSet.FirstOrDefault(compiledcriteria);
            }
            return entity;
        }

        public T FindFirstOfDefault(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes)
        {
            if (predicate == null)
                throw new ArgumentNullException("predicate");

            if (includes != null)
            {
                foreach (var path in includes)
                {
                    DbSet.Include(this.GetPropertyName<T>(path));
                }
            }

            var entity = DbSet.Local.FirstOrDefault(predicate.Compile());
            if (entity == null)
            {
                entity = DbSet.FirstOrDefault(predicate.Compile());
            }
            return entity;
        }


        public IQueryable<T> GetAll()
        {
            return Entities;
        }

        public IQueryable<T> Find(Expression<Func<T, bool>> criteria, string[] includes = null)
        {
            if (includes != null)
            {
                foreach (string property in includes)
                {
                    DbSet.Include(property);
                }
            }
            return DbSet.Where(criteria);
        }

        public IQueryable<T> Find<TKey>(Expression<Func<T, bool>> criteria, Expression<Func<T, TKey>> sort,
                                        bool desc = false, string[] includes = null)
        {
            if (criteria == null)
                throw new ArgumentNullException("criteria");

            if (includes == null)
                return sort != null
                           ? desc ? DbSet.Where(criteria).OrderByDescending(sort) : DbSet.Where(criteria).OrderBy(sort)
                           : DbSet.Where(criteria);
            foreach (string property in includes)
            {
                DbSet.Include(property);
            }
            return sort != null
                       ? desc ? DbSet.Where(criteria).OrderByDescending(sort) : DbSet.Where(criteria).OrderBy(sort)
                       : DbSet.Where(criteria);
        }

        public int Count(Expression<Func<T, bool>> predicate)
        {
            return predicate != null ? DbSet.Count(predicate) : DbSet.Count();
        }
        public bool Any(Expression<Func<T, bool>> predicate)
        {
            return predicate != null ? DbSet.Any(predicate) : DbSet.Any();
        }

        protected IDbSet<T> Entities
        {
            get { return DbSet ?? (DbSet = DbContext.Set<T>()); }
        }
        protected IDbSet<T> NewEntities
        {
            get { return DbContext.Set<T>(); }
        }
        public void Refresh(T instance)
        {
            DbContext.Entry<T>(instance).Reload();
        }

        public DataTable AsDataTable()
        {
            var table = new DataTable();

            var props = TypeDescriptor.GetProperties(typeof(T))
                                          .Cast<PropertyDescriptor>()
                                          .Where(propertyInfo => propertyInfo.PropertyType.Namespace.Equals("System"))
                                          .ToArray();

            foreach (var propertyInfo in props)
            {
                table.Columns.Add(propertyInfo.Name, Nullable.GetUnderlyingType(propertyInfo.PropertyType) ?? propertyInfo.PropertyType);
            }

            return table;
        }

        public string GetPropertyName<T>(Expression<Func<T, object>> property)
        {
            LambdaExpression lambda = (LambdaExpression)property;
            MemberExpression memberExpression;

            if (lambda.Body is UnaryExpression)
            {
                UnaryExpression unaryExpression = (UnaryExpression)(lambda.Body);
                memberExpression = (MemberExpression)(unaryExpression.Operand);
            }
            else
            {
                memberExpression = (MemberExpression)(lambda.Body);
            }

            return ((PropertyInfo)memberExpression.Member).Name;
        }

        public void ExecuteRawSql(string sql)
        {
            throw new NotImplementedException();
        }

        public T GetOneFromSql(string sql, params object[] parameters)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> GetManyFromSql(string sql, params object[] parameters)
        {
            throw new NotImplementedException();
        }
    }
}
