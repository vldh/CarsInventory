namespace NhatH.MVC.CarInventory.Core.Service
{
    public interface ICarInventoryServiceFactory
    {
        T Get<T>() where T : ICarInventoryService;
    }
}
