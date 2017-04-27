using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyStoreWebUI.Infastructure
{
    public class UserIP
    {
        public string GetClientIpaddress()
        {
            string ipAddress = string.Empty;
            ipAddress = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (ipAddress == "" || ipAddress == null)
            {
                ipAddress = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                return ipAddress;
            }
            else
            {
                return ipAddress;
            }
        }

    }
}