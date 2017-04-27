using System.Collections.Generic;

using MyStoreDomain.Entities;

namespace MyStoreDomain.Abstract
{
    public interface IitemsRepository
    {
        IEnumerable<MyItem> Myitems { get;}  
        void SaveProduct(MyItem product);  
        MyItem DeleteProduct(int productID);
    }

    
}
