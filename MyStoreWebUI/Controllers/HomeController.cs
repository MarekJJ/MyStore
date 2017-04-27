using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyStoreDomain.Abstract;
using MyStoreDomain.Entities;
using MyStoreWebUI.Models;
using System.Collections;
using MyStoreDomain.Concrete;
using MyStoreWebUI.Infastructure;

namespace MyStoreWebUI.Controllers
{
    public class HomeController : Controller // progam start
    {
        private IitemsRepository repository;
        public int PageSize = 9;

        public object NumberID { get; private set; }

        public HomeController(IitemsRepository productRepository)//ninject Associates parameter, from data base
        {
            this.repository = productRepository;
        }
        public ViewResult List(string category, int page = 1)
         {
            //--  user ip should  get--------------------
            UserIP userIP = new UserIP();
            EmailOrderProcessor email = new EmailOrderProcessor(new EmailSettings());
            email.FromServerPayPal(userIP.GetClientIpaddress());
            //--------------------------------------------
            ProductsListViewModel viewModel = new ProductsListViewModel
            {                                            
                MyItems = repository.Myitems.Where(p => category == null || p.Category == category).OrderBy(p => p.NumberID).Skip((page - 1) * PageSize).Take(PageSize),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = PageSize,

                   TotalItems = category == null ? repository.Myitems.Count() : repository.Myitems.Where(e => e.Category == category).Count()
                },                                 
                CurrentCategory = category
            };
            return View(viewModel);
        }
        public FileContentResult GetImage(int NumberID)
        {
         MyItem prod = repository.Myitems.FirstOrDefault(p => p.NumberID == NumberID);
              if (prod != null)
              {
                return File(prod.ImageData, prod.ImageMimeType);
              }
              else
              {
                return null;
              }
        }

        public ViewResult OneItem(int numberID)  // selecting one item
        {
           MyItem item = repository.Myitems.Single(p => p.NumberID == numberID);
           return View("OneItem", item);
        }

        public ViewResult Search(string search, int page = 1) //serching  item for user
        {
            ProductsListViewModel viewModel = new ProductsListViewModel
            {
                MyItems = repository.Myitems.Where(p => p.Name1.ToUpper() == search.ToUpper() || p.Category.ToUpper() == search.ToUpper()),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = PageSize,
                    TotalItems = repository.Myitems.Where(p => p.Name1.ToUpper() == search.ToUpper() || p.Category.ToUpper() == search.ToUpper()).Count()
                },           
            };
            return View("List", viewModel);
        }



    }
}