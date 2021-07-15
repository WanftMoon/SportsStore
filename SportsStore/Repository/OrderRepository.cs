using Microsoft.EntityFrameworkCore;
using SportsStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsStore.Repository
{
	public class OrderRepository : IOrderRepository
	{
		private StoreDbContext _context;
		
		public OrderRepository(StoreDbContext ctx)
		{
			_context = ctx;
		}
		
		public IQueryable<Order> Orders => _context.Orders.Include(o => o.Lines).ThenInclude(l => l.Product);
		
		
		public void SaveOrder(Order order)
		{
			_context.AttachRange(order.Lines.Select(l => l.Product));
			
			if (order.OrderID == 0)
				_context.Orders.Add(order);
			
			_context.SaveChanges();
		}
	}
}
