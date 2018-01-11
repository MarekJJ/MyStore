using System.Data.Entity;
using MyStoreDomain.Entities;


namespace MyStoreDomain.Concrete
{
    public class SfcDb : DbContext // conection to data base 
    {
        public DbSet<MyItem> myItems { get; set; }
    }
}
//SfcDb