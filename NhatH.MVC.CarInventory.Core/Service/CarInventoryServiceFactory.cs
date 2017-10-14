using NhatH.MVC.CarInventory.Core.Framework.IoC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NhatH.MVC.CarInventory.Core.Service
{
    public class CarInventoryServiceFactory:ICarInventoryServiceFactory
    {
        public T Get<T>() where T : ICarInventoryService
        {
            var service = AutofacIocAdapter.Instance.GetService<T>();
            if (service != null)
            {
                return service;
            }

            return default(T);
        }
    }
}
