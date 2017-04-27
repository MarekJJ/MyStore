using MyStoreDomain.Entities;

namespace MyStoreDomain.Abstract
{
  public  interface IOrderProcessor
    {
       void ProcessOrder(Cart cart, ShippingDetails shippingDetails);
    }
}
