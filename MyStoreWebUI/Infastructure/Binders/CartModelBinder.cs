
using System.Web.Mvc;
using MyStoreDomain.Entities;

namespace MyStoreWebUI.Infastructure.Binders
{
    public class CartModelBinder : IModelBinder //creating session of all entitis of Class in solution, this is global
    {
        private const string sessionKey = "Cart";
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {                    
            Cart cart = null;
            if (controllerContext.HttpContext.Session != null)  
            {                                                   
                cart = (Cart)controllerContext.HttpContext.Session[sessionKey];  
            }
            
            if (cart == null) 
            {
                cart = new Cart(); 
                        
                if (controllerContext.HttpContext.Session != null) 
                {
                    controllerContext.HttpContext.Session[sessionKey] = cart; 
                }
            }
            return cart; 
        }
    }
}

