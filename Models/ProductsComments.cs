using System.ComponentModel.DataAnnotations.Schema;

namespace CuaHangDoAn.Models
{
    public class ProductsComments
    {
        public int Id { get; set; } 
        public string Comment { get; set; }
        public DateTime CreatedDate { get; set; }   
        public int Rating { get; set; }
        public int ProductID { get; set; }
        [ForeignKey("ProductID")]
        public virtual Product? Product { get; set; }
    }
}
