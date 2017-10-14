using NhatH.MVC.CarInventory.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NhatH.MVC.CarInventory.DB
{
    public class CarInventoryDBContext : DbContext
    {
        public CarInventoryDBContext(string connectionString) : base(connectionString) { }
        public CarInventoryDBContext() : base("DefaultConnection") { }

        public DbSet<Car> Cars { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            IEnumerable<Type> typesToRegister =
                Assembly.GetExecutingAssembly()
                    .GetTypes()
                    .Where(type => !string.IsNullOrEmpty(type.Namespace))
                    .Where(
                        type =>
                        type.BaseType != null && type.BaseType.IsGenericType
                        && type.BaseType.GetGenericTypeDefinition() == typeof(EntityTypeConfiguration<>));
            foreach (Type type in typesToRegister)
            {
                dynamic configurationInstance = Activator.CreateInstance(type);
                modelBuilder.Configurations.Add(configurationInstance);
            }

            base.OnModelCreating(modelBuilder);
        }
    }
}
