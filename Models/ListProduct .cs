using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace StrugariSiminaLab7.Models
{
    public class ListProduct
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        // Foreign key for the ShopList table
        [ForeignKey(typeof(ShopList))]
        public int ShopListID { get; set; }

        // Foreign key for the Product table
        [ForeignKey(typeof(Product))]
        public int ProductID { get; set; }

        // Navigational property for ShopList
        [ManyToOne]
        public ShopList ShopList { get; set; }

        // Navigational property for Product
        [ManyToOne]
        public Product Product { get; set; }
    }
}
