namespace NhatH.MVC.CarInventory.Core.Service.Contract
{
    public interface IAuthorizationService : ICarInventoryService
    {
        bool HasPermission(string functionName);
        //PermissionSet GetPermissions(string functionName);
        bool IsSuperAdmin();
        bool IsSuperAdmin(string user);
    }
}
