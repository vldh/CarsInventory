using NhatH.MVC.CarInventory.Domain;
using System.Data.Entity.ModelConfiguration;

namespace NhatH.MVC.CarInventory.DB.Mapping
{
    public class UserMapping : EntityTypeConfiguration<User>
    {
        public UserMapping()
        {
            ToTable("User");
            HasKey(u => u.ID);
            Property(u => u.UserName).IsRequired().IsUnicode(true).HasMaxLength(255);
        }
    }
}
