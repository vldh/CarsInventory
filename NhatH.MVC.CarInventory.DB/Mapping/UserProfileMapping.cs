using NhatH.MVC.CarInventory.Domain;
using System.Data.Entity.ModelConfiguration;

namespace NhatH.MVC.CarInventory.DB.Mapping
{
    public class UserProfileMapping : EntityTypeConfiguration<UserProfile>
    {
        public UserProfileMapping()
        {
            ToTable("UserProfile");
            HasKey(u => u.ID);
            Property(u => u.UserGuid).IsRequired();
            Property(u => u.Email).IsUnicode(false).HasMaxLength(255);
            Property(u => u.Telephone).IsUnicode(false).HasMaxLength(50);
            Property(u => u.Mobile).IsUnicode(false).HasMaxLength(50);
        }
    }
}
