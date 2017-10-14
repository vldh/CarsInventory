using NhatH.MVC.CarInventory.Core.Core.Model.Mapper.Inventory;
using NhatH.MVC.CarInventory.Core.Core.Model.Type;
using NhatH.MVC.CarInventory.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NhatH.MVC.CarInventory.Core.Service.Contract
{
    public interface ICarService : ICarInventoryService
    {
        IQueryable<Car> GetCars();
        bool InsertedCar(CarModel model);
        bool UpdateCar(CarModel carModel);
        bool DeleteCar(int carId);
    }
}
