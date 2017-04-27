using System.Collections.Generic;
using System.Web.Mvc;
using MyStoreDomain.Abstract;
using System.Linq;
using MyStoreDomain.Entities;

namespace MyStoreWebUI.Controllers
{
    public class NavController : Controller // this is menu with categories
    {
        private IitemsRepository repository;
        public NavController(IitemsRepository repo)//ninject Associates parameter, from data base
        {
            repository = repo;
        }
        public PartialViewResult Menu(string category = null)
        {
            ViewBag.SelectedCategory = category;
            IEnumerable<string> categories = repository.Myitems.Select(x => x.Category).Distinct().OrderBy(x => x);
            return PartialView("Menu", categories);
        }

        public PartialViewResult Me(MyItem item)
        {
            return PartialView("Me", item);
        }

        
    }
}