using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
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
		private const string adminUser = "Admin";
		private const string adminPassword = "Secret123$";

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

		public static async void EnsureMigrations(this IApplicationBuilder app)
		{
			StoreDbContext ctx = app.ApplicationServices
							.CreateScope()
							.ServiceProvider
							.GetRequiredService<StoreDbContext>();

			if (ctx.Database.GetPendingMigrations().Any())
				ctx.Database.Migrate();

			AppIdentityDbContext idCtx = app.ApplicationServices
				.CreateScope()
				.ServiceProvider
				.GetRequiredService<AppIdentityDbContext>();

			if (idCtx.Database.GetPendingMigrations().Any())
				idCtx.Database.Migrate();

			//creates the admin
			UserManager<IdentityUser> userManager = app.ApplicationServices
				.CreateScope().ServiceProvider
				.GetRequiredService<UserManager<IdentityUser>>();

			IdentityUser user = await userManager.FindByIdAsync(adminUser);

			if (user == null)
			{
				user = new IdentityUser("Admin");
				user.Email = "admin@example.com";
				user.PhoneNumber = "555-1234";
				await userManager.CreateAsync(user, adminPassword);
			}
		}
	}
}
