using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SportsStore.Models;
using SportsStore.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsStore.Extensions
{
	public static class AppExtensions
	{
		public static void PopulateDb(this IApplicationBuilder app)
		{
			StoreDbContext ctx = app.ApplicationServices
				.CreateScope()
				.ServiceProvider
				.GetRequiredService<StoreDbContext>();

			if (ctx.Database.GetPendingMigrations().Any())
				ctx.Database.Migrate();

			if (!ctx.Products.Any())
			{
				ctx.Products.AddRange(
					new Product()
					{
						Name = "Kayak",
						Description = "A boat for one person",
						Category = "Watersports",
						Price = 275m
					},
					new Product()
					{
						Name = "Lifejacket",
						Description = "Protective and fashionable",
						Category = "Watersports",
						Price = 48.95m
					},
					new Product()
					{
						Name = "Soccer Ball",
						Description = "FIFA-approved size and weight",
						Category = "Soccer",
						Price = 19.5m
					},
					new Product()
					{
						Name = "Corner Flags",
						Description = "Give your playing field a professional touch",
						Category = "Soccer",
						Price = 34.95m
					});
				ctx.SaveChanges();
			}
		}
	}
}
