using System;             // this is for admin login
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyStoreWebUI.Infastructure.Abstract
{
   public interface IAuthProvider // implementaion FormsAuthProvider
    {
        bool Authenticate(string username, string password);
    }
}
