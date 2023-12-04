using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CuaHangDoAn.Models
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }




        public ICollection<OrderDetails>? OrderDetails { get; set; }


        public decimal TotalPrice { get; set; }
        public string UserID { get; set; }
        [ForeignKey("UserID")]
        public virtual AppUser? AppUser { get; set; }
    }
}
