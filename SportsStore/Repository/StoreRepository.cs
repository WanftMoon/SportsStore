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
	}
}
