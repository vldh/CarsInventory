using NhatH.MVC.CarInventory.DB;
using System;
using System.Configuration;
using System.Data.Entity;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;

namespace NhatH.MVC.CarInventory.Core.Framework.DatabaseConfiguration
{
    public class InstallDatabase
    {
        private readonly string _nameOrConnectionString;
        public InstallDatabase(string nameOrConnectionString)
        {
            _nameOrConnectionString = nameOrConnectionString;
        }

        public void InitializerDatabase()
        {
            var tablesToValidate = new[] { "" };

            var MigrationAutomatic = ConfigurationManager.AppSettings["MigrationAutomatic"] != null ? Convert.ToBoolean(ConfigurationManager.AppSettings["MigrationAutomatic"]) : false;

            //initialize database the firsttime of development
            var initializer = new CreateTablesIfNotExist<CarInventoryDBContext>(tablesToValidate);
            //initialize every migration file in the migration folder
            var initializerMigration = new MigrateDatabaseToLatestVersion<CarInventoryDBContext, DB.Migrations.Configuration>();
            if (MigrationAutomatic)
                System.Data.Entity.Database.SetInitializer(initializerMigration);
            else
                System.Data.Entity.Database.SetInitializer(initializer);
        }

        public void CreateStoreProcedures(string strStore)
        {
            StringBuilder sb = new StringBuilder();

            using (var conn = new SqlConnection(_nameOrConnectionString))
            {
                conn.Open();
                using (var command = new SqlCommand(strStore, conn))
                {
                    command.ExecuteNonQuery();
                }
            }

        }

        public void CreateDatabase()
        {
            if (SqlServerDatabaseExists(_nameOrConnectionString))
                return;

            try
            {
                //parse database name

                
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private bool SqlServerDatabaseExists(string connectionString)
        {
            try
            {
                //just try to connect
                using (var conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public void UpdateDatabase()
        {
            var folderScript = "DatabaseScripts/UpdateDB";
            var path = Path.Combine(AppDomain.CurrentDomain.RelativeSearchPath, folderScript);
            if (!Directory.Exists(path)) {
                return;
            }
            var files = Directory.EnumerateFiles(path).ToArray();
            foreach (var file in files)
            {
                if (File.Exists(file))
                {
                    var sql = File.ReadAllText(file);
                    CreateStoreProcedures(sql);
                }
            }
        }
    }
}
