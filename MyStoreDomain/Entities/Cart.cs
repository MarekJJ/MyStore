//Full of this class is in session, reading is from IEnumerable<CartLine> Lines
using System;
using System.Collections.Generic;
using System.Linq;
namespace MyStoreDomain.Entities
{
    public class Cart
    {
        private List<CartLine> lineCollection = new List<CartLine>();  //List with storing items 

        public void AddItem(MyItem product, int quantity) // adding to Cart
        {
            CartLine line = lineCollection.Where(p => p.Product.NumberID == product.NumberID).FirstOrDefault(); // checking if element existing if true adding it to list lineCollection  
            if (line == null) 
            {
                lineCollection.Add(new CartLine { Product = product, Quantity = quantity }); // dodaje do listy lineCollection kolejny element
            }
            else // element existing already on list so increase only quantity
            {
                line.Quantity += quantity;  
            }
        }

        public void RemoveLine(MyItem product) // delete from list selected element 
        {
            lineCollection.RemoveAll(l => l.Product.NumberID == product.NumberID);
        }

        public decimal ComputeTotalValue() // this metod is called in fiew palaces returning sum of all prices on list 
        {
            return lineCollection.Sum(e => e.Product.Price * e.Quantity);
        }

        

        public void Clear()   // clear list
        {
            lineCollection.Clear();
        }

        public IEnumerable<CartLine> Lines // returning elements from list when is called 
        {
            get { return lineCollection; }
        }



    }
    public class CartLine    // class for list lineCollection
    {
        public MyItem Product { get; set; }
        public int Quantity { get; set; }
    }
}