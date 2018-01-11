using System.Linq;
using System.Web.Mvc;
using MyStoreDomain.Abstract;
using MyStoreDomain.Entities;//dfsaf
using MyStoreWebUI.Models;

namespace MyStoreWebUI.Controllers
{
    public class CartController : Controller
    {
        public IitemsRepository repository;
        public IOrderProcessor orderProcessor;
        public CartController(IitemsRepository repo, IOrderProcessor proc)//ninject Associates parameters
        {
            repository = repo;
            orderProcessor = proc;
        }

        public ViewResult Checkout(Cart cart) // entity cart is in session
        {

            foreach (var p in repository.Myitems) // checking if client doesn't added more items than is available if more than reducing to available quantity
            {
                foreach (var i in cart.Lines)
                {
                    if (p.NumberID == i.Product.NumberID)
                    {
                        int bd = int.Parse(p.amount);

                        if (bd < i.Quantity)
                        {
                            i.Quantity = bd;
                            return View("Delete", p);
                        }
                    }
                }
            }
            return View(new ShippingDetails());
        }

        [HttpPost]
        public ViewResult Checkout(Cart cart, ShippingDetails shippingDetails)
        {
            if (cart.Lines.Count() == 0)
            {
                ModelState.AddModelError("", "Koszyk jest pusty!");
            }
            if (ModelState.IsValid)
            {
                orderProcessor.ProcessOrder(cart, shippingDetails);

                return View("Completed");
            }
            else
            {
                return View(shippingDetails);
            }
        }

        public PartialViewResult Summary(Cart cart)
        {
            return PartialView(cart);
        }

        public ViewResult Index(Cart cart, string returnUrl) //returnUrl this is URL from view
        {
            return View(new CartIndexViewModel { ReturnUrl = returnUrl, Cart = cart });
        }


        public RedirectToRouteResult AddToCart(Cart cart, int productId, string returnUrl)
        {
            MyItem product = repository.Myitems.FirstOrDefault(p => p.NumberID == productId);
            if (product != null)
            {
                cart.AddItem(product, 1);
            }

            return RedirectToAction("Index", new { returnUrl });
        }

        public RedirectToRouteResult RemoveFromCart(Cart cart, int productId, string returnUrl)// removeing from cart which is in session
        {
            MyItem product = repository.Myitems.FirstOrDefault(p => p.NumberID == productId);
            if (product != null)
            {
                cart.RemoveLine(product);
            }
            return RedirectToAction("Index", new { returnUrl });
        }

        public void Clear(Cart cart)
        {
            cart.Clear();
        }
    }
}