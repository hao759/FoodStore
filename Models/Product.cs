using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CuaHangDoAn.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        //public string Description { get; set; }
        //public string? Image { get; set; }
        //[NotMapped]
        //[DisplayName("Upload File")]
        //public IFormFile? ImageFile { get; set; }
        public int CateID { get; set; }
        [ForeignKey("CateID")]
        public virtual CategoryProduct? CategoryProduct { get; set; }
    }
}
