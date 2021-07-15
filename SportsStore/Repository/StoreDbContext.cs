using Microsoft.EntityFrameworkCore;
using SportsStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsStore.Repository
{
	public class StoreDbContext : DbContext
	{
		public StoreDbContext(DbContextOptions<StoreDbContext> opt) : base(opt)
		{
			int i = 0;
		}

		public DbSet<Product> Products { get; set; }
		public DbSet<Order> Orders { get; set; }

	}
}
