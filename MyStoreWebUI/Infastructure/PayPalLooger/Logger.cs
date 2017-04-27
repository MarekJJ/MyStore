using MyStoreDomain.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
                                                                 
namespace MyStoreWebUI.Infastructure.PayPalLooger// saveing exeptions to file my_app.log also sending  errors to class EmailOrderProces with sending to my email 
{
    public class Logger
    {
         public static string LogDirectoryPath = Environment.CurrentDirectory;

        public static void Log(String lines)
        {
            try
            {
                EmailOrderProcessor em = new EmailOrderProcessor(new EmailSettings());
                em.FromServerPayPal("Error" + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + " --> " + lines);

                System.IO.StreamWriter file = new System.IO.StreamWriter(LogDirectoryPath + "\\Error.log", true);
                file.WriteLine(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + " --> " + lines);
                file.Close();
            }
            catch
            {

            }
        }
    }
}