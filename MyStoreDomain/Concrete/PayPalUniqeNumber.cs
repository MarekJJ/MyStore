using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MyStoreDomain.Concrete // crating unique number for paypal
{

   public class PayPalUniqeNumber :System.Web.UI.Page/* this is necessary to use Server.MapPath  */
    {
        
        public string ProductNumberToFile()
        {

            StreamReader stream = null;

            

            stream = File.OpenText(Server.MapPath(@"~/TransactionInPayPalID.txt")); // this is path to project MyStoreWebUI
            int number = int.Parse(stream.ReadToEnd());            
            number++;
            stream.Close();
                    
            StreamWriter steramWriter = new StreamWriter(Server.MapPath(@"~/TransactionInPayPalID.txt"));
            StringBuilder text = new StringBuilder();

            text.AppendLine(number.ToString());

            steramWriter.WriteLine(text.ToString()); 

            steramWriter.Close();

            return number.ToString();


        }
    }
}
