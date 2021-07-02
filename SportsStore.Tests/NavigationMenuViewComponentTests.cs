using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Moq;
using SportsStore.Components;
using SportsStore.Models;
using SportsStore.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SportsStore.Tests
{
	public class NavigationMenuViewComponentTests
	{
		[Fact]
		public void Can_Select_Categories()
		{
			//arrange
			Mock<IStoreRepository> mock = new Mock<IStoreRepository>();

			mock.Setup(m => m.Products).Returns(
				Enumerable.Range(1, 5)
						 .Select((i) => new Product()
						 {
							 ProductId = i,
							 Name = $"p{i}",
							 Category = $"Cat{i}"
						 }).AsQueryable<Product>()
			);

			NavigationMenuViewComponent target = new(mock.Object);

			//act
			var result = (IEnumerable<string>)((ViewViewComponentResult)target.Invoke()).ViewData.Model;

			//assert
			string[] ar = result.ToArray();

			Assert.True(Enumerable.SequenceEqual(new string[] { "Cat1", "Cat2", "Cat3", "Cat4", "Cat5" }, ar));

		}

		[Fact]
		public void Indicates_Selected_Category()
		{
			//arrange
			string categoryToSelect = "p1";
			Mock<IStoreRepository> mock = new Mock<IStoreRepository>();

			mock.Setup(m => m.Products).Returns(
				Enumerable.Range(1, 5)
						 .Select((i) => new Product()
						 {
							 ProductId = i,
							 Name = $"p{i}",
							 Category = $"Cat{i}"
						 }).AsQueryable<Product>()
			);

			NavigationMenuViewComponent target = new(mock.Object)
			{
				ViewComponentContext = new()
				{
					ViewContext = new()
					{
						RouteData = new()
					}
				}
			};

			target.RouteData.Values["category"] = categoryToSelect;

			//act
			var result = (string)((ViewViewComponentResult)target.Invoke()).ViewData["SelectedCategory"];

			//assert
			Assert.Equal(categoryToSelect, result);

		}
	}
}
