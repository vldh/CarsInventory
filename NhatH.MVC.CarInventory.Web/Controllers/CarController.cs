using NhatH.MVC.CarInventory.Core.Core.Model.Mapper.Inventory;
using NhatH.MVC.CarInventory.Core.Service.Contract;
using NhatH.MVC.CarInventory.DB.UoW;
using NhatH.MVC.CarInventory.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NhatH.MVC.CarInventory.Web.Controllers
{
    public class CarController : Controller
    {
        private readonly ICarService _carService;
        public CarController(ICarService carService)
        {
            this._carService = carService;
        }
        // GET: Car
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetUserCars()
        {
            return Json(new { cars = _carService.GetCars() }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeleteCar(int carId)
        {
            _carService.DeleteCar(carId);
            return Json(new { result = true });
        }

        public JsonResult InsertCar(CarModel car)
        {
            _carService.InsertedCar(car);
            return Json(new { result = true });
        }

        public JsonResult EditCar(CarModel car)
        {
            _carService.UpdateCar(car);
            return Json(new { result = true });
        }
    }
}