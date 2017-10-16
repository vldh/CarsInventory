using System.ComponentModel;

namespace NhatH.MVC.CarInventory.Core.Core.Model.Mapper.Inventory
{
    public class CarModel: BaseModel
    {
        [DisplayName("CarModel.Brand")]
        public string Brand { get; set; }
        [DisplayName("CarModel.Model")]
        public string Model { get; set; }
        [DisplayName("CarModel.Year")]
        public int Year { get; set; }
        [DisplayName("CarModel.Price")]
        public decimal Price { get; set; }
        [DisplayName("CarModel.New")]
        public bool IsNew { get; set; }
    }
}
