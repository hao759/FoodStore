using Microsoft.AspNetCore.Identity;

namespace CuaHangDoAn.Models
{
    public class AppUser: IdentityUser
    {
        public ICollection<Order>? Orders { get; set; }
    }
}
