using System.ComponentModel.DataAnnotations.Schema;

namespace CuaHangDoAn.Models
{
    public class OrderDetails
    {
        public int ProductID { get; set; }
        [ForeignKey("ProductID")]
        public virtual Product? Product { get; set; }



        public int OrderID { get; set; }
        [ForeignKey("OrderID")]
        public virtual Order? Order { get; set; }

        public int quantity { get; set; }
    }
}
