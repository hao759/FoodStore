using System.ComponentModel.DataAnnotations;

namespace CuaHangDoAn.Models
{
    public class CategoryProduct
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        //[Remote(action: "Validate", controller: "CategoryProducts", areaName: "Admin")]//AdditionalFields = "Password,RePassword", HttpMethod = "GET"
        public string Description { get; set; }
        public ICollection<Product>? Products { get; set; }
    }
}
