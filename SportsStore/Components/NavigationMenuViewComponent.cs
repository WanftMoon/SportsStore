using Microsoft.AspNetCore.Mvc;
using SportsStore.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsStore.Components
{
	public class NavigationMenuViewComponent : ViewComponent
	{
		private IStoreRepository _repository;

		public NavigationMenuViewComponent(IStoreRepository repo)
		{
			_repository = repo;
		}

		public IViewComponentResult Invoke()
		{
			ViewBag.SelectedCategory = RouteData?.Values["category"];

			return View(_repository.Products
				.Select(x => x.Category)
				.Distinct()
				.OrderBy(x => x));
		}
	}
}
