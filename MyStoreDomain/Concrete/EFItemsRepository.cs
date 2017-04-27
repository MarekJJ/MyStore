using System.Collections.Generic;
using MyStoreDomain.Abstract;
using MyStoreDomain.Entities;
using MyStoreDomain.Concrete;

using System;

namespace MyStore.Domain.Concrete
{           
    public class EFItemsRepository : IitemsRepository  // implementation interface which have IEnumerable Myitems 
    {                                                 
        private DbItemsContext context = new DbItemsContext();// conection to data base entity 

        public IEnumerable<MyItem> Myitems {get { return context.myItems; }}

        public void SaveProduct(MyItem product) // to save new next element of list
        {
            if (product.NumberID == 0) // if is 0 element is new not edited  
            {
                context.myItems.Add(product);// adding as a new element
            }
            else
            {
                MyItem dbEntry = context.myItems.Find(product.NumberID); // find element with ID
                                                                       
                if (dbEntry != null) //  if existing then  element is editing by admin
                {
                    dbEntry.Name1 = product.Name1; 
                    dbEntry.Description = product.Description;
                    dbEntry.Price = product.Price;
                    dbEntry.Category = product.Category;
                    dbEntry.ImageData = product.ImageData;
                    dbEntry.ImageMimeType = product.ImageMimeType;
                    dbEntry.amount = product.amount;
                }
            }
            context.SaveChanges(); // save changes in data base
        }
        public MyItem DeleteProduct(int productID) // Deleteing element
        {
            MyItem dbEntry = context.myItems.Find(productID); // dbEntry bedzie wskazywac na dysku ten produkt
            if (dbEntry != null) 
            {
                context.myItems.Remove(dbEntry); //
                context.SaveChanges();           // save changes
            }
            return dbEntry; 
        }

        
    }
}


