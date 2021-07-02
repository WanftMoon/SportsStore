using SportsStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsStore.Repository
{
	public interface IStoreRepository
	{
		IQueryable<Product> Products { get; }
	}
}
