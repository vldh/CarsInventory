using NhatH.MVC.CarInventory.Core.Core.Helper;
using NhatH.MVC.CarInventory.Core.Core.Model.Mapper.Inventory;
using NhatH.MVC.CarInventory.Core.Service.Contract;
using NhatH.MVC.CarInventory.DB.UoW;
using NhatH.MVC.CarInventory.Domain;
using System;
using System.Linq;
using System.Threading;
using System.Web;

namespace NhatH.MVC.CarInventory.Core.Service.Impl
{
    public class CarService : ICarService
    {
        private readonly ICarInventoryUoW _carInventoryUow;
        public CarService(ICarInventoryUoW carInventoryUoW)
        {
            _carInventoryUow = carInventoryUoW;
        }
        public IQueryable<Car> GetCars()
        {
            string userName = HttpContext.Current.User.Identity.Name.ToLower(Thread.CurrentThread.CurrentCulture);
            return _carInventoryUow.Car.Find(c => c.User == userName).OrderBy(c => c.Brand).ThenBy(c => c.Model);
        }

        public bool InsertedCar(CarModel model)
        {
            try
            {
                string userName = HttpContext.Current.User.Identity.Name.ToLower(Thread.CurrentThread.CurrentCulture);
                _carInventoryUow.Car.Insert(new Car()
                {
                    Model = model.Model,
                    Brand = model.Brand,
                    IsNew = model.IsNew,
                    Price = model.Price,
                    Year = model.Year,
                    User = userName
                });
                _carInventoryUow.Commit();
            }
            catch (Exception exc)
            {
                exc.Error("An error was occurred when inserting a car");
                return false;
            }
            return true;
        }

        public bool UpdateCar(CarModel model)
        {
            try
            {
                string userName = HttpContext.Current.User.Identity.Name.ToLower(Thread.CurrentThread.CurrentCulture);
                var car = _carInventoryUow.Car.FindFirstOfDefault(c => c.ID == model.Id && c.User == userName);
                if (car != null)
                {
                    car.Model = model.Model;
                    car.Brand = model.Brand;
                    car.IsNew = model.IsNew;
                    car.Price = model.Price;
                    car.Year = model.Year;
                    _carInventoryUow.Car.Update(car);
                    _carInventoryUow.Commit();
                }

            }
            catch (Exception exc)
            {
                exc.Error("An error was occurred when updating an existing car");
                return false;
            }
            return true;
        }
        public bool DeleteCar(int carId)
        {
            try
            {
                string userName = HttpContext.Current.User.Identity.Name.ToLower(Thread.CurrentThread.CurrentCulture);
                var car = _carInventoryUow.Car.FindFirstOfDefault(c => c.ID == carId && c.User == userName);
                if (car != null)
                {
                    _carInventoryUow.Car.Delete(car);
                    _carInventoryUow.Commit();
                }

            }
            catch (Exception exc)
            {
                exc.Error("An error was occurred when deleting an existing car");
                return false;
            }
            return true;
        }
    }
}
