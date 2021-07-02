using Microsoft.AspNetCore.Mvc;
using Moq;
using SportsStore.Controllers;
using SportsStore.Models;
using SportsStore.Models.ViewModels;
using SportsStore.Repository;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace SportsStore.Tests
{
	public class HomeControllerTests
	{
		[Fact]
		public void Can_Use_Repository()
		{
			//arrange
			Mock<IStoreRepository> mock = new Mock<IStoreRepository>();

			mock.Setup(m => m.Products).Returns(new Product[] {
				new Product {ProductId = 1, Name = "p1"},
				new Product {ProductId = 2, Name = "p2"}
			}.AsQueryable<Product>());

			HomeController ctlr = new HomeController(mock.Object);

			//act
			ProductsListViewModel result = (ProductsListViewModel)((ViewResult)ctlr.Index(null)).ViewData.Model;

			//assert
			Product[] ar = result.Products.ToArray();

			Assert.True(ar.Length == 2);
			Assert.Equal("p1", ar[0].Name);
			Assert.Equal("p2", ar[1].Name);
		}

		[Fact]
		public void Can_Send_Pagination_View_Model()
		{
			//arrange
			Mock<IStoreRepository> mock = new Mock<IStoreRepository>();

			mock.Setup(m => m.Products).Returns(new Product[] {
				new Product {ProductId = 1, Name = "p1"},
				new Product {ProductId = 2, Name = "p2"},
				new Product {ProductId = 3, Name = "p3"},
				new Product {ProductId = 4, Name = "p4"},
				new Product {ProductId = 5, Name = "p5"},

			}.AsQueryable<Product>());

			HomeController ctlr = new HomeController(mock.Object) { PageSize = 3 };

			//act
			ProductsListViewModel result = (ProductsListViewModel)((ViewResult)ctlr.Index(null, 2)).ViewData.Model;

			//assert
			PagingInfo pInfo = result.PagingInfo;

			Assert.Equal(2, pInfo.CurrentPage);
			Assert.Equal(3, pInfo.ItemsPerPage);
			Assert.Equal(5, pInfo.TotalItems);
			Assert.Equal(2, pInfo.TotalPages);
		}


		[Fact]
		public void Can_Paginate()
		{
			//arrange
			Mock<IStoreRepository> mock = new Mock<IStoreRepository>();

			mock.Setup(m => m.Products).Returns(new Product[] {
				new Product {ProductId = 1, Name = "p1"},
				new Product {ProductId = 2, Name = "p2"},
				new Product {ProductId = 3, Name = "p3"},
				new Product {ProductId = 4, Name = "p4"},
				new Product {ProductId = 5, Name = "p5"}

			}.AsQueryable<Product>());

			HomeController ctlr = new HomeController(mock.Object) { PageSize = 3 };

			//act
			ProductsListViewModel result = (ProductsListViewModel)((ViewResult)ctlr.Index(null, 2)).ViewData.Model;

			//assert
			Product[] ar = result.Products.ToArray();

			Assert.True(ar.Length == 2);
			Assert.Equal("p4", ar[0].Name);
			Assert.Equal("p5", ar[1].Name);
		}

		[Fact]
		public void Can_Filter_Products()
		{
			//Arrange
			Mock<IStoreRepository> mock = new Mock<IStoreRepository>();

			mock.Setup(m => m.Products).Returns(
				Enumerable.Range(1, 5)
						 .Select((i) => new Product()
						 {
							 ProductId = i,
							 Name = $"p{i}",
							 Category = $"Cat{i % 2 + 1}"
						 }).AsQueryable<Product>()
			);

			HomeController ctlr = new HomeController(mock.Object) { PageSize = 3 };

			//act
			ProductsListViewModel result = (ProductsListViewModel)((ViewResult)ctlr.Index("Cat2", 1)).ViewData.Model;

			//assert
			Product[] ar = result.Products.ToArray();

			Assert.True(ar.Length == 3);
			Assert.True(ar[0].Name.Equals("p1") && ar[0].Category == "Cat2");
			Assert.True(ar[1].Name.Equals("p3") && ar[0].Category == "Cat2");
		}

		[Fact]
		public void Generate_Category_Specific_Product_Count()
		{
			//Arrange
			Mock<IStoreRepository> mock = new Mock<IStoreRepository>();

			mock.Setup(m => m.Products).Returns(
				Enumerable.Range(1, 5)
						 .Select((i) => new Product()
						 {
							 ProductId = i,
							 Name = $"p{i}",
							 Category = $"Cat{i % 2 + 1}"
						 }).AsQueryable<Product>()
			);

			HomeController ctlr = new HomeController(mock.Object) { PageSize = 3 };

			//act
			ProductsListViewModel result = (ProductsListViewModel)((ViewResult)ctlr.Index("Cat2", 1)).ViewData.Model;

			//assert
			Product[] ar = result.Products.ToArray();

			Assert.True(ar.Length == 3);
			Assert.True(ar[0].Name.Equals("p1") && ar[0].Category == "Cat2");
			Assert.True(ar[1].Name.Equals("p3") && ar[0].Category == "Cat2");
		}

	}
}
