using NhatH.MVC.CarInventory.Core.Core.Model.Mapper.Inventory;
using NhatH.MVC.CarInventory.Core.Service.Contract;
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
           var action =  _carService.DeleteCar(carId);
            return Json(new { result = action });
        }

        public JsonResult InsertCar(CarModel car)
        {
            var action = _carService.InsertedCar(car);
            return Json(new { result = action });
        }

        public JsonResult EditCar(CarModel car)
        {
            var action =_carService.UpdateCar(car);
            return Json(new { result = action });
        }
    }
}