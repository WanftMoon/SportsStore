using Microsoft.AspNetCore.Mvc;
using SportsStore.Models.ViewModels;
using SportsStore.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsStore.Controllers
{
	public class HomeController : Controller
	{
		private IStoreRepository _repository;
		public int PageSize { get; set; } = 2;

		public HomeController(IStoreRepository repo)
		{
			_repository = repo;
		}

		public IActionResult Index(string category, int prodPage = 1)
		{
			var prods = _repository.Products
								 .Where(x => category == null || x.Category.ToLower().Equals(category.ToLower()))
								 .OrderBy(x => x.ProductId)
								 .Skip((prodPage - 1) * PageSize)
								 .Take(PageSize);
			int total = category == null ? _repository.Products.Count() : prods.Count();

			return View(new ProductsListViewModel()
			{
				Products = prods,
				PagingInfo = new PagingInfo()
				{
					CurrentPage = prodPage,
					ItemsPerPage = PageSize,
					TotalItems = total
				},
				CurrentCategory = category
			});
		}
	}
}
