using SportsStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SportsStore.Tests
{
	public class CartTests
	{
		[Fact]
		public void Can_Add_New_Lines()
		{
			//arrange
			var products = Enumerable.Range(1, 5)
								 .Select((i) => new Product()
								 {
									 ProductId = i,
									 Name = $"p{i}",
									 Category = $"Cat{i}"
								 });

			Cart target = new Cart();

			//act
			foreach (var item in products)
				target.AddItem(item, 1);

			var results = target.Lines;
			//assert
			Assert.Equal(5, results.Count);
			Assert.Equal(1, results[0].Product.ProductId);
			Assert.Equal(2, results[1].Product.ProductId);
		}

		[Fact]
		public void Can_Add_Quantity_For_Existing_Line()
		{
			//arrange
			var products = Enumerable.Range(1, 5)
								 .Select((i) => new Product()
								 {
									 ProductId = i,
									 Name = $"p{i}",
									 Category = $"Cat{i}"
								 });

			Cart target = new Cart();

			//act
			foreach (var item in products)
				target.AddItem(item, 1);

			target.AddItem(target.Lines[0].Product, 10);

			var results = target.Lines;
			//assert
			Assert.Equal(5, results.Count);
			Assert.Equal(11, results[0].Quantity);
			Assert.Equal(1, results[1].Quantity);
		}

		[Fact]
		public void Can_Remove_Line()
		{
			//arrange
			var products = Enumerable.Range(1, 5)
								 .Select((i) => new Product()
								 {
									 ProductId = i,
									 Name = $"p{i}",
									 Category = $"Cat{i}"
								 });

			Cart target = new Cart();

			//act
			foreach (var item in products)
				target.AddItem(item, 1);

			target.RemoveLine(target.Lines[0].Product);

			var results = target.Lines;
			//assert
			Assert.Equal(4, results.Count);
		}


		[Fact]
		public void Calculate_Cart_Total()
		{
			//arrange
			var products = Enumerable.Range(1, 5)
								 .Select((i) => new Product()
								 {
									 ProductId = i,
									 Name = $"p{i}",
									 Category = $"Cat{i}",
									 Price = 10m * i
								 });

			Cart target = new Cart();

			//act
			foreach (var item in products)
				target.AddItem(item, 1);
						
			var result = target.ComputeTotalValue();
			//assert
			Assert.Equal(150m, result);
		}

		[Fact]
		public void Can_Clear_Contents()
		{
			//arrange
			var products = Enumerable.Range(1, 5)
								 .Select((i) => new Product()
								 {
									 ProductId = i,
									 Name = $"p{i}",
									 Category = $"Cat{i}",
									 Price = 10m * i
								 });

			Cart target = new Cart();

			//act
			foreach (var item in products)
				target.AddItem(item, 1);

			target.Clear();
			//assert
			Assert.Empty(target.Lines);
		}
	}
}
