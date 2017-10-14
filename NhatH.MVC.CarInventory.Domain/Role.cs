using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NhatH.MVC.CarInventory.Domain
{
    public class Role:BaseEntity
    {
        public string RoleName { get; set; }
        public string Description { get; set; }
        public bool IsActived { get; set; }
        public bool? IsNotDeletable { get; set; }
        public DateTime? ModifyDate { get; set; }
        public string ModifyBy { get; set; }

        private ICollection<User> _users;
        public virtual ICollection<User> Users
        {
            get { return _users ?? (_users = new List<User>()); }
            set { _users = value; }
        }

        private ICollection<RoleFunction> _roleFunctions;
        public virtual ICollection<RoleFunction> RoleFunctions
        {
            get { return _roleFunctions ?? (_roleFunctions = new List<RoleFunction>()); }
            set { _roleFunctions = value; }
        }
    }
}
