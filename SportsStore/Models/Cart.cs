using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsStore.Models
{
	public class Cart
	{
		public List<CartLine> Lines { get; set; } = new();

		public virtual void AddItem(Product product, int quantity)
		{
			CartLine line = Lines.Where(x => x.Product.ProductId == product.ProductId).FirstOrDefault();

			if (line == null)
				Lines.Add(new CartLine()
				{
					Product = product,
					Quantity = quantity
				});
			else
				line.Quantity += quantity;
		}

		public virtual void RemoveLine(Product product)
		{
			Lines.RemoveAll(l => l.Product.ProductId == product.ProductId);
		}

		public virtual decimal ComputeTotalValue()
		{
			return Lines.Sum(x => x.Product.Price * x.Quantity);
		}

		public virtual void Clear() => Lines.Clear();
	}
}
