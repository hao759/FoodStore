using CuaHangDoAn.ModelView;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using CuaHangDoAn.ModelView;

namespace CuaHangDoAn.Views.Home.Components.YourOrder
{
    public class YourOrder : ViewComponent
    {
        public Cart? Cart { get; private set; }

        public IViewComponentResult Invoke()
        {
            Cart cart = GetCart();
            return View(cart);
        }


        public Cart GetCart()
        {
            var sessionCart = HttpContext.Session.GetString("Cart");
            if (sessionCart != null)
                Cart = JsonConvert.DeserializeObject<Cart>(sessionCart);
            else
            {
                Cart = new Cart();
                Cart.Items = new List<CartItem>();
            }
            return Cart;
        }
    }


}
