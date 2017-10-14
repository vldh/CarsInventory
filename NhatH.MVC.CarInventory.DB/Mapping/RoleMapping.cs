using NhatH.MVC.CarInventory.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NhatH.MVC.CarInventory.DB.Mapping
{
    public class RoleMapping:EntityTypeConfiguration<Role>
    {
        public RoleMapping() {
            ToTable("Role");
            HasKey(u => u.ID);
            Property(u => u.RoleName).IsRequired().IsUnicode(true).HasMaxLength(50);

            HasMany(c => c.Users)
               .WithMany(c => c.Roles)
               .Map(c =>
               {
                   c.ToTable("UserRole");
                   c.MapLeftKey("RoleId");
                   c.MapRightKey("UserId");
               });
        }
    }
}
