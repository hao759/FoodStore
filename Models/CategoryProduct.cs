using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CuaHangDoAn.Models
{
    public class CategoryProduct
    {
        [Key]
        public int Id { get; set; }
        [RegularExpression(@"^[a-z0-9-]*$", ErrorMessage = "Chỉ dùng các ký tự [a-z0-9-]")]
        [DisplayName("Tên sản phẩm")]
        public string Name { get; set; }
        [Remote(action: "Validate", controller: "CategoryProducts", areaName: "Admin")]//AdditionalFields = "Password,RePassword", HttpMethod = "GET"
        [DisplayName("Mô tả")]
        public string Description { get; set; }
        public ICollection<Product>? Products { get; set; }
    }
}
