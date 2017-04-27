using MyStoreDomain.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;


namespace MyStoreDomain.Concrete
{
    public class DbItemsContext : DbContext // conection to data base 
    {
        public DbSet<MyItem> myItems {  get; set; } 
    }            
}
