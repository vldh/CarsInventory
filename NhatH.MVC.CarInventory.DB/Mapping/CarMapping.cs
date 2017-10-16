using NhatH.MVC.CarInventory.Domain;
using System.Data.Entity.ModelConfiguration;

namespace NhatH.MVC.CarInventory.DB.Mapping
{
    public class CarMapping:EntityTypeConfiguration<Car>
    {
        public CarMapping()
        {
            ToTable("Cars");
            HasKey(c => c.ID);
            Property(c => c.Brand).IsRequired().HasMaxLength(100);
            Property(c => c.Model).IsRequired().HasMaxLength(100);
            Property(c => c.Year).IsOptional();
            Property(c=>c.Price).IsOptional();
            Property(c => c.IsNew).IsOptional();
            Property(c => c.User).IsOptional();
        }
    }
}
