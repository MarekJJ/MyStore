﻿using PayPal.Api; 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyStoreDomain.Entities;
using log4net.Repository.Hierarchy;
using MyStoreDomain.Abstract;
using MyStoreDomain.Concrete;
using System.Text;
using System.IO;

namespace MyStoreWebUI.Controllers 
{
    public class PaypalController : Controller
    {

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult PaymentWithPaypal()
        {
            //getting the apiContext as earlier
            APIContext apiContext = Configuration.GetAPIContext();

            try
            {
                string payerId = Request.Params["PayerID"];

                if (string.IsNullOrEmpty(payerId))
                {
                    //this section will be executed first because PayerID doesn't exist

                    //it is returned by the create function call of the payment class

                    // Creating a payment

                    // baseURL is the url on which paypal sendsback the data.

                    // So we have provided URL of this controller only

                    string baseURI = Request.Url.Scheme + "://" + Request.Url.Authority + "/Paypal/PaymentWithPayPal?";

                    //guid we are generating for storing the paymentID received in session

                    //after calling the create function and it is used in the payment execution

                    var guid = Convert.ToString((new Random()).Next(100000));

                    //CreatePayment function gives us the payment approval url

                    //on which payer is redirected for paypal acccount payment

                    var createdPayment = this.CreatePayment(apiContext, baseURI + "guid=" + guid);

                    //get links returned from paypal in response to Create function call

                    var links = createdPayment.links.GetEnumerator();

                    string paypalRedirectUrl = null;

                    while (links.MoveNext())
                    {
                        Links lnk = links.Current;

                        if (lnk.rel.ToLower().Trim().Equals("approval_url"))
                        {
                            //saving the payapalredirect URL to which user will be redirected for payment
                            paypalRedirectUrl = lnk.href;
                        }
                    }

                    // saving the paymentID in the key guid
                    Session.Add(guid, createdPayment.id);

                    return Redirect(paypalRedirectUrl);
                }
                else
                {
                    // This section is executed when we have received all the payments parameters

                    // from the previous call to the function Create

                    // Executing a payment

                    var guid = Request.Params["guid"];

                    var executedPayment = ExecutePayment(apiContext, payerId, Session[guid] as string);

                    if (executedPayment.state.ToLower() != "approved")
                    {
                        return View("FailureView");
                    }

                }
            }
            catch (Exception ex) //Excepion opisuje wiele bledow  rzeszta rodzajow exceptoin dziediczy z tej
            {
                 Infastructure.PayPalLooger.Logger.Log("Error" + ex.Message);
                 return View("FailureView" );
            }
            Cart cart = (Cart)HttpContext.Session["Cart"]; // udalo sie tak zapodac tutaj sesje
            return View("SuccessView", cart);
        }

      
        private PayPal.Api.Payment payment;


        private Payment ExecutePayment( APIContext apiContext, string payerId, string paymentId)
        {
            var paymentExecution = new PaymentExecution() { payer_id = payerId };
            this.payment = new Payment() { id = paymentId };
            return this.payment.Execute(apiContext, paymentExecution);
        }

        private Payment CreatePayment(APIContext apiContext, string redirectUrl)
        {
            Cart cart = (Cart)HttpContext.Session["Cart"]; // cart from session

            //similar to credit card create itemlist and add item objects to it
            ItemList itemlist = new ItemList(){ items = new List<Item>() };
            double sum = 0;
            double con = 0;
            
            foreach (var p in cart.Lines)
            {
                con = (double)p.Product.Price;
                itemlist.items.Add(new Item(){name = p.Product.Name1.ToString(), currency = "PLN", price =con.ToString(), quantity = "1", sku = "sku" });
                sum += (double)p.Product.Price;
            }
            var itemList = itemlist;
            

            var payer = new Payer() { payment_method = "paypal" };

            // Configure Redirect Urls here with RedirectUrls object
            var redirUrls = new RedirectUrls()
            {
                cancel_url = redirectUrl,
                return_url = redirectUrl
            };

            // similar as we did for credit card, do here and create details object
            var details = new Details()
            {
                tax = "1",
                shipping = "1",
                subtotal =sum.ToString() // here is sum of all prices
            };
            double dupa = sum + 2;
            // similar as we did for credit card, do here and create amount object
            var amount = new Amount()
            {    
                currency = "PLN",
                total =  dupa.ToString(), //this is sum to all objesct tax and shipping
                details = details
            };

            var transactionList = new List<Transaction>();
            PayPalUniqeNumber uniqeNunber = new PayPalUniqeNumber();
            transactionList.Add(new Transaction()
            {
               description = "Transaction description.",
                invoice_number = uniqeNunber.ProductNumberToFile(), /*here is , uniqe number */
                amount = amount,
                item_list = itemList
            });

            this.payment = new Payment()
            {
                intent = "sale",
                payer = payer,
                transactions = transactionList,
                redirect_urls = redirUrls
            };

            // Create a payment using a APIContext
            return this.payment.Create(apiContext);

        }

       



       
    }
}
