﻿using Microsoft.AspNetCore.Mvc;
using SportsStore.Models;
using SportsStore.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsStore.Controllers
{
	public class OrderController : Controller
	{
		private IOrderRepository repository;

		private Cart cart;

		public OrderController(IOrderRepository repoService, Cart cartService)
		{
			repository = repoService;
			cart = cartService;
		}

		public IActionResult Checkout()
		{
			return View(new Order());
		}

		[HttpPost]
		public IActionResult Checkout(Order order)
		{
			if (cart.Lines.Count() == 0)
			{
				ModelState.AddModelError("", "Sorry, your cart is empty!");
			}

			if (ModelState.IsValid)
			{
				order.Lines = cart.Lines.ToArray();
				repository.SaveOrder(order);
				cart.Clear();

				return RedirectToPage("/Order/Completed", new { orderId = order.OrderID });
			}

			return View();

		}
	}
}