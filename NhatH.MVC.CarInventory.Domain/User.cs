using System.Collections.Generic;

namespace NhatH.MVC.CarInventory.Domain
{
    public class User : BaseEntity
    {
        public string UserName { get; set; }

        private ICollection<Role> _roles;

        public virtual ICollection<Role> Roles
        {
            get { return _roles ?? (_roles = new List<Role>()); }
            set { _roles = value; }
        }

        private ICollection<UserProfile> _userProfiles;

        public virtual ICollection<UserProfile> UserProfiles
        {
            get { return _userProfiles ?? (_userProfiles = new List<UserProfile>()); }
            set { _userProfiles = value; }
        }
    }
}
