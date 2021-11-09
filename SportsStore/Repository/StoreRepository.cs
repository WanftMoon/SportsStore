using SportsStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsStore.Repository
{
	public class StoreRepository : IStoreRepository
	{
		private StoreDbContext _context;

		public StoreRepository(StoreDbContext ctx)
		{
			_context = ctx;
		}

		public IQueryable<Product> Products => _context.Products;

		public void CreateProduct(Product p)
		{
			_context.Add(p);
			_context.SaveChanges();
		}

		public void DeleteProduct(Product p)
		{
			_context.Remove(p);
			_context.SaveChanges();
		}

		public void SaveProduct(Product p)
		{
			_context.SaveChanges();
		}
	}
}
