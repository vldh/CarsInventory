using System;

namespace NhatH.MVC.CarInventory.Domain
{
    public class UserProfile:BaseEntity
    {
        public Guid UserGuid { get; set; }
        public int? UserId { get; set; }
        public string UserName { get; set; }
        public string Token { get; set; }

        public string Name { get; set; }
        public string Email { get; set; }
        public string Telephone { get; set; }
        public string Mobile { get; set; }
        public string Avatar { get; set; }
        public bool IsActived { get; set; }
        public bool? IsNotDeletable { get; set; }
        public DateTime? ModifyDate { get; set; }
        public string ModifyBy { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? LastChangePassword { get; set; }

        public string DecimalSymbol { get; set; }
        public string ThousandSymbol { get; set; }
        public string DateFormat { get; set; }
        public DateTime? RoleChangedDate { get; set; }
    }
}
