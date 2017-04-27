using System.Collections.Generic;
using MyStoreDomain.Entities;
namespace MyStoreWebUI.Models
{
    public class ProductsListViewModel
    {
        public IEnumerable<MyItem> MyItems { get; set; }
        public PagingInfo PagingInfo { get; set; }
        public string CurrentCategory { get; set; }
    }
}