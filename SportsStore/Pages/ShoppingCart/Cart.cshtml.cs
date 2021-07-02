using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SportsStore.Extensions;
using SportsStore.Models;
using SportsStore.Repository;

namespace SportsStore.Pages.ShoppingCart
{
    public class CartModel : PageModel
    {
        private IStoreRepository _repository;

		public Cart Cart { get; set; }
		public string ReturnUrl { get; set; }

		public CartModel(IStoreRepository repository)
		{
            _repository = repository;
		}

        public void OnGet(string returnUrl)
        {
            ReturnUrl = returnUrl ?? "/";
            Cart = HttpContext.Session.GetJson<Cart>("cart") ?? new Cart();
        }

        public IActionResult OnPost(long productId, string returnUrl)
		{
            Product product = _repository.Products.FirstOrDefault(x => x.ProductId == productId);

            Cart = HttpContext.Session.GetJson<Cart>("cart") ?? new Cart();

            Cart.AddItem(product, 1);

            HttpContext.Session.SetJson("cart", Cart);

            return RedirectToPage(new { returnUrl = returnUrl });
		}
    }
}
