using NhatH.MVC.CarInventory.Domain;
using System.Web.Security;

namespace NhatH.MVC.CarInventory.Core.Service.Contract
{
    public interface IAuthenticationService : ICarInventoryService
    {
        bool SignIn(string userName, string password, bool persistCookie = false);
        void SignOut(string userName);
        UserProfile GetAuthenticatedUserProfile();
        UserProfile GetAuthenticatedUserProfile(FormsAuthenticationTicket ticket);
        bool IsLoggedin();
    }
}
