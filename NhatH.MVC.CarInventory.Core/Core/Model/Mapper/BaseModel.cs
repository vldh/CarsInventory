using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NhatH.MVC.CarInventory.Core.Core.Model.Mapper
{
    public class BaseModel
    {
        public int Id { get; set; }
        [DisplayName("Name")]
        public string Name { get; set; }
        public bool IsNotEditable { get; set; }
    }
}
