using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SportsStore.Extensions;
using SportsStore.Models;
using SportsStore.Repository;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace SportsStore
{
	public class Startup
	{
		public IConfiguration Configuration { get; }

		public Startup(IConfiguration cfg)
		{
			Configuration = cfg;

			CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("en-US");
		}

		// This method gets called by the runtime. Use this method to add services to the container.
		// For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddControllersWithViews();
			services.AddDbContext<StoreDbContext>(opt =>
			{
				opt.UseSqlServer(Configuration["ConnectionStrings:DefaultConnection"]);
			});
			services.AddScoped<IStoreRepository, StoreRepository>();
			services.AddScoped<IOrderRepository, OrderRepository>();

			services.AddRazorPages()
#if DEBUG
				.AddRazorRuntimeCompilation();
#else
				;
#endif
			services.AddDistributedMemoryCache();
			services.AddSession();
			services.AddScoped<Cart>(sp => SessionCart.GetCart(sp));
			services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
			services.AddServerSideBlazor();

			services.AddDbContext<AppIdentityDbContext>(options =>
			{
				options.UseSqlServer(Configuration["ConnectionStrings:IdentityConnection"]);
			});
			services.AddIdentity<IdentityUser, IdentityRole>()
				.AddEntityFrameworkStores<AppIdentityDbContext>();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsProduction())
			{
				app.UseExceptionHandler("/error");
			}
			else
			{
				app.UseDeveloperExceptionPage();
				app.UseStatusCodePages();
			}

			app.UseStatusCodePages();
			app.UseStaticFiles();
			app.UseSession();
			app.UseRouting();

			//must appear between routing and use endpoints
			app.UseAuthentication();
			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllerRoute("catpage",
					"{category}/Page{prodPage}", new { Controller = "Home", action = "Index" });
				endpoints.MapControllerRoute("page",
					"Page{prodPage}", new { Controller = "Home", action = "Index", prodPage = 1 });
				endpoints.MapControllerRoute("category",
					"{category}", new { Controller = "Home", action = "Index", prodPage = 1 });
				endpoints.MapControllerRoute("pagination",
					"Products/Page{prodPage}", new { Controller = "Home", action = "Index", prodPage = 1 });
				endpoints.MapDefaultControllerRoute();
				endpoints.MapRazorPages();
				endpoints.MapBlazorHub();
				endpoints.MapFallbackToPage("/admin/{catchall}", "/Admin/Index");
			});

#if DEBUG
			app.PopulateDb();
#endif
			app.EnsureMigrations();
		}
	}
}
