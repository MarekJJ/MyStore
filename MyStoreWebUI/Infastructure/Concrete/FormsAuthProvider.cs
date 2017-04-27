using System.Web.Security;
using MyStoreWebUI.Infastructure.Abstract;// it is for login
namespace MyStoreWebUI.Infastructure.Concrete
{
    public class FormsAuthProvider : IAuthProvider
    {
        public bool Authenticate(string username, string password)
        {
            bool result = FormsAuthentication.Authenticate(username, password);// this is out of date, not safe
            if (result)
            {
                FormsAuthentication.SetAuthCookie(username, false);
            }
            return result;
        }
    }
}