using NhatH.MVC.CarInventory.Core.Core.Model.Mapper.Function;
using System.Collections.Generic;

namespace NhatH.MVC.CarInventory.Core.Service.Contract
{
    public interface IFunctionService : ICarInventoryService
    {
        bool UpdateFunctionsToRole(int roleId, string[] functions);
        void DeleteAllFunction();
        void ClearCacheFunction(string userName);
        IList<RoleFunctionModel> GetFunctionsByUser(string userName);
        void DeleteAllMenu();
        string GetModuleByFunctionKey(string functionKey);
        IEnumerable<string> GetFunctionNeedChecked();
        IEnumerable<string> RequireFunctions();
    }
}
