using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace CuaHangDoAn.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [DisplayName("Tên sản phẩm")]
        public string Name { get; set; }
        //public string Description { get; set; }

        [DisplayName("Giá sản phẩm")]
        public decimal? Price { get; set; }


        public string? Image { get; set; }
        [NotMapped]
        [DisplayName("Ảnh sản phẩm")]
        public IFormFile? ImageFile { get; set; }


        [DisplayName("Loại sản phẩm")]
        public int CateID { get; set; }
        [ForeignKey("CateID")]
        public virtual CategoryProduct? CategoryProduct { get; set; }
    }
}
