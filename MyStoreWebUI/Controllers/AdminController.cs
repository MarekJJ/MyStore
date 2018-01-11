using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyStoreDomain.Abstract;
using MyStoreDomain.Entities;

namespace MyStoreWebUI.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private IitemsRepository repository;
        public AdminController(IitemsRepository repo) //ninject Associates parameter
        {
            repository = repo;
        }
        public ViewResult Index()
        {
            return View(repository.Myitems);
        }
        public ViewResult Edit(int productId)
        {
            MyItem product = repository.Myitems.FirstOrDefault(p => p.NumberID == productId);
            return View(product);
        }


        [HttpPost]
        public ActionResult Edit(MyItem product, HttpPostedFileBase image = null)
        {
            if (ModelState.IsValid) // if everything  is filled
            {
                if (image != null) //  jst image
                {
                    product.ImageMimeType = image.ContentType;
                    product.ImageData = new byte[image.ContentLength];
                    image.InputStream.Read(product.ImageData, 0, image.ContentLength);
                }

                repository.SaveProduct(product);
                TempData["message"] = string.Format("Zapisano {0}", product.Name1);  // showing what got added
                return RedirectToAction("Index");
            }
            else
            {
                // if are problems to send
                return View(product);
            }
        }

        public ViewResult Create()
        {
            return View("Edit", new MyItem());// you can send empty
        }

        [HttpPost]
        public ActionResult Delete(int productId)
        {
            MyItem deletedProduct = repository.DeleteProduct(productId);
            if (deletedProduct != null)
            {
                TempData["message"] = string.Format("Usunięto {0}", deletedProduct.Name1);
            }
            return RedirectToAction("Index");
        }
    }
}