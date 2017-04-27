// z concrete seu takie dane jak email lczenie z baza danych  repository itp
using System.Net;
using System.Net.Mail;
using System.Text;
using MyStoreDomain.Abstract;
using MyStoreDomain.Entities;
using System.Web.Helpers;
using System.Web;
using System.Web.Mvc;

namespace MyStoreDomain.Concrete     // sending emails to me
{

    public class EmailSettings
    {
        public string MailToAddress = "marekjedrysiak1@gmail.com"; //  OK
        public string MailFromAddress = "mail@marekjedrysiak.com";//OK
        public bool UseSsl = true;//OK
        public string Username = "mail@marekjedrysiak.com"; // OK
        public string Password = "Civilization4/";
        public string ServerName = "mailuk2.promailserver.com";//ok
        public int ServerPort = 25; //OK
        public bool WriteAsFile = true; // OK
        public string FileLocation = @"c:\sports_store_emails";//OK
    }


    public class EmailOrderProcessor : IOrderProcessor 
    {
        private EmailSettings emailSettings;

        

        public EmailOrderProcessor(EmailSettings settings)
        {
            emailSettings = settings;  
        }

        
        public void ProcessOrder(Cart cart, ShippingDetails shippingInfo)// shipping details of customer if bought something
        {
           
            WebMail.SmtpServer = emailSettings.ServerName;
            WebMail.SmtpPort = 25;
            WebMail.EnableSsl = true;
            WebMail.UserName = emailSettings.Username;
            WebMail.Password = emailSettings.Password;
            WebMail.From = emailSettings.MailFromAddress;

            StringBuilder body = new StringBuilder().AppendLine("Nowe zamówienie <br/>").AppendLine("Produkty: <br/>");
            foreach (var line in cart.Lines)
            {
                var subtotal = line.Product.Price * line.Quantity;
                body.AppendFormat("{0} x {1} (wartość: {2:c}) <br/>", line.Quantity,line.Product.Name1, subtotal);
            }
            body.AppendFormat("<br/>Wartość całkowita: {0:c}", cart.ComputeTotalValue() + "<br/>")
            .AppendLine("Wysyłka dla: <br/>")
            .AppendLine(shippingInfo.Name +"<br/>")
            .AppendLine(shippingInfo.Line1 + "<br/>")
            .AppendLine(shippingInfo.Line2 + "<br/>" ?? "" + "<br/>")
            .AppendLine(shippingInfo.Line3 + "<br/>" ?? "" + "<br/>")

            .AppendLine(" Miasto: ")
            .AppendLine(shippingInfo.City + "<br/>")

            .AppendLine("Wojewódctwo: ")
            .AppendLine(shippingInfo.State + "<br/>" ?? "" + "<br/>")

            .AppendLine("Kraj: ")
            .AppendLine(shippingInfo.Country)
            .AppendLine("Kod Pocztowy :")
            .AppendLine(shippingInfo.Zip + "<br/>")
            .AppendLine("--- <br/>")
            .AppendFormat("Pakowanie prezentu: {0}", shippingInfo.GiftWrap ? "Tak" : "Nie");


            WebMail.Send(emailSettings.MailToAddress, "Message from server", body.ToString()); // wysyla do mnie maila 

            emailSettings.MailToAddress = shippingInfo.Email; // ten wysyal do klijeta
            StringBuilder body1 = new StringBuilder();
            body1.AppendLine("<body style =\"width:900px; height:auto; margin: 0,auto,0,auto;\"><div style =\"text-align:center;\"><h1 style =\"color:blue;\"> Potwierdzenie zamówienia </h1><p> Dziękuję za zamówienie produktu, należy jeszcze za niego zaplacić ;).</p><p> Jeżeli już zapłaciłeś zignoruj te wiadomość. </p></div></body> ");
            body1.AppendFormat(body.ToString());                                    
            WebMail.Send(emailSettings.MailToAddress, "Message from server", body1.ToString());
            body.Clear();
            body1.Clear();
        }

        public void ContactWotchMe(MessageToMe message)// if someone want to write to me
        {
            WebMail.SmtpServer = emailSettings.ServerName;
            WebMail.SmtpPort = 25;
            WebMail.EnableSsl = true;
            WebMail.UserName = emailSettings.Username;
            WebMail.Password = emailSettings.Password;
            WebMail.From = emailSettings.MailFromAddress;

            StringBuilder body = new StringBuilder();

            if (message.Company == null && message.Company == null && message.Localization == null) //custumer Asking fo CV
            {
                body.AppendLine("Ktoś chce CV od ciebie  <br/> Imie: <br/>" + message.Name + "<br/> Nazwisko: <br/>" + message.SurName + "<br/> email: <br/>" + message.Email);
            }
            else //Custumer writin something 
            {
                body.AppendLine("Ktoś napisał do ciebie <br/> Imie: <br/>" + message.Name + "<br/> Nazwisko: <br/>" + message.SurName + "<br/> email: <br/>" + message.Email + "<br/> Firma <br/>" + message.Company + "<br/> Lokalizacja <br/>" + message.Localization + "<br/> Wiadomość <br/>" + message.WriteToMe);
            }
            WebMail.Send(emailSettings.MailToAddress, "Message from server", body.ToString()); // 

            body.Clear();
        }

        public void FromServerPayPal(string message)// sending message to my email if is exeption in paypal controler
        {
            WebMail.SmtpServer = emailSettings.ServerName;
            WebMail.SmtpPort = 25;
            WebMail.EnableSsl = true;
            WebMail.UserName = emailSettings.Username;
            WebMail.Password = emailSettings.Password;
            WebMail.From = emailSettings.MailFromAddress;

            
           
            WebMail.Send("marekjedrysiak1@gmail.com", "Message from Paypal", message); 

            
        }



    }
}

//Wskazówka Jeżeli nie masz dostępnego serwera SMTP, nie przejmuj się tym.Jeśli ustawisz wartość true
//właściwości EmailSettings.WriteAsFile, wiadomości poczty elektronicznej będą zapisywane jako pliki do katalogu
//zdefiniowanego we właściwości FileLocation.Katalog ten musi istnieć i mieć nadane uprawnienia do zapisu.
//Pliki będą zapisane z rozszerzeniem.eml, ale można je odczytać w dowolnym edytorze tekstu. W omawianym
//przykładzie wskazano katalog c:\sports_store_emails.



