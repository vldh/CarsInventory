using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Transactions;

namespace NhatH.MVC.CarInventory.Core.Framework.DatabaseConfiguration
{
    public class CreateTablesIfNotExist<TContext> : IDatabaseInitializer<TContext> where TContext : DbContext
    {
        private readonly string[] _tablesToValidate;

        public CreateTablesIfNotExist(string[] tablesToValidate)
        {
            _tablesToValidate = tablesToValidate;
        }

        public void InitializeDatabase(TContext context)
        {
            bool dbExists;
            using (new TransactionScope(TransactionScopeOption.Suppress))
            {
                dbExists = context.Database.Exists();
            }
            if (dbExists)
            {
                bool createTables = false;
                if (_tablesToValidate != null && _tablesToValidate.Length > 0)
                {
                    //we have some table names to validate
                    var existingTableNames =
                        new List<string>(
                            context.Database.SqlQuery<string>(
                                "SELECT table_name FROM INFORMATION_SCHEMA.TABLES WHERE table_type = 'BASE TABLE'"));
                    createTables =
                        !existingTableNames.Intersect(_tablesToValidate, StringComparer.InvariantCultureIgnoreCase)
                                           .Any();
                }
                else
                {
                    //check whether tables are already created
                    int numberOfTables = 0;
                    foreach (
                        int t1 in
                            context.Database.SqlQuery<int>(
                                "SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE table_type = 'BASE TABLE' "))
                        numberOfTables = t1;

                    createTables = numberOfTables == 0;
                }

                if (createTables)
                {
                    //create all tables
                    string dbCreationScript = ((IObjectContextAdapter)context).ObjectContext.CreateDatabaseScript();
                    context.Database.ExecuteSqlCommand(dbCreationScript);

                    //Seed(context);
                    context.SaveChanges();
                }
            }
            else
            {
                throw new ApplicationException("No database instance");
            }
        }
    }
}
