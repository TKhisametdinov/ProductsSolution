using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http.Results;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Products.WebApi.Controllers;
using Products.WebApi.Interfaces;
using Products.WebApi.Models;

namespace Products.WebApi.Tests.Controllers
{
    [TestClass]
    public class ProductControllerTest
    {
        private const int Id = 1;

        [TestMethod]
        public async Task GetReturnAllProducts()
        {
            // Arrange
            var mockRepository = new Mock<IRepository<Product>>();
            mockRepository.Setup(
                x =>
                x.GetAsync(
                    It.IsAny<Expression<Func<Product, bool>>>(),
                    It.IsAny<Func<IQueryable<Product>, IOrderedQueryable<Product>>>(),
                    It.IsAny<string>()))
                .Returns(
                    Task.FromResult(
                        new List<Product>() { new Product { ID = 1 }, new Product { ID = 2 }, new Product { ID = 3 } }));
            var controller = new ProductsController(mockRepository.Object);

            // Act
            var actionResult = await controller.GetProducts();

            // Assert
            Assert.IsNotNull(actionResult);
            Assert.AreEqual(3, actionResult.Count());
        }

        [TestMethod]
        public async Task GetReturnsProductWithSameId()
        {
            // Arrange
            var mockRepository = new Mock<IRepository<Product>>();
            mockRepository.Setup(x => x.GetByIdAsync(Id)).Returns(Task.FromResult(new Product { ID = Id }));
            var controller = new ProductsController(mockRepository.Object);

            // Act
            var actionResult = await controller.GetProduct(Id);
            var contentResult = actionResult as OkNegotiatedContentResult<Product>;

            // Assert
            Assert.IsNotNull(contentResult);
            Assert.IsNotNull(contentResult.Content);
            Assert.AreEqual(Id, contentResult.Content.ID);
        }

        [TestMethod]
        public async Task PutProduct()
        {
            // Arrange
            var mockRepository = new Mock<IRepository<Product>>();
            mockRepository.Setup(x => x.Update(It.IsAny<Product>()));
            mockRepository.Setup(x => x.SaveChangesAsync()).Returns(Task.FromResult(1));
            var controller = new ProductsController(mockRepository.Object);

            // Act
            var actionResult =
                await
                controller.PutProduct(
                    Id,
                    new Product { ID = Id, LastUpdated = DateTime.Now, Name = "sony", PhotoData = null, Price = 596 });
            var contentResult = actionResult as StatusCodeResult;

            // Assert
            Assert.IsNotNull(contentResult);
            Assert.AreEqual(HttpStatusCode.NoContent, contentResult.StatusCode);
        }

        [TestMethod]
        public async Task PutProductNotFound()
        {
            // Arrange
            var mockRepository = new Mock<IRepository<Product>>();
            mockRepository.Setup(
                x =>
                x.GetAsync(
                    It.IsAny<Expression<Func<Product, bool>>>(),
                    It.IsAny<Func<IQueryable<Product>, IOrderedQueryable<Product>>>(),
                    It.IsAny<string>())).Returns(Task.FromResult(new List<Product>()));
            mockRepository.Setup(x => x.Update(It.IsAny<Product>()));
            mockRepository.Setup(x => x.SaveChangesAsync()).Throws<DbUpdateConcurrencyException>();
            var controller = new ProductsController(mockRepository.Object);

            // Act
            var actionResult =
                await
                controller.PutProduct(
                    Id,
                    new Product { ID = Id, LastUpdated = DateTime.Now, Name = "sony", PhotoData = null, Price = 596 });

            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task PutProductBadRequest()
        {
            // Arrange
            var mockRepository = new Mock<IRepository<Product>>();
            var controller = new ProductsController(mockRepository.Object);

            // Act
            var actionResult =
                await
                controller.PutProduct(
                    Id + 1,
                    new Product { ID = Id, LastUpdated = DateTime.Now, Name = "sony", PhotoData = null, Price = 596 });

            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(BadRequestResult));
        }

        [TestMethod]
        public async Task PostProduct()
        {
            // Arrange
            var mockRepository = new Mock<IRepository<Product>>();
            mockRepository.Setup(x => x.Add(It.IsAny<Product>()));
            mockRepository.Setup(x => x.SaveChangesAsync()).Returns(Task.FromResult(1));
            var controller = new ProductsController(mockRepository.Object);

            // Act
            var actionResult =
                await
                controller.PostProduct(
                    new Product { ID = Id, LastUpdated = DateTime.Now, Name = "sony", PhotoData = null, Price = 596 });
            var contentResult = actionResult as CreatedAtRouteNegotiatedContentResult<Product>;

            // Assert
            Assert.IsNotNull(contentResult);
            Assert.IsNotNull(contentResult.RouteValues);
            Assert.AreEqual(Id, contentResult.RouteValues["id"]);
        }

        [TestMethod]
        public async Task DeleteProduct()
        {
            // Arrange
            var mockRepository = new Mock<IRepository<Product>>();
            mockRepository.Setup(x => x.GetByIdAsync(Id)).Returns(Task.FromResult(new Product { ID = Id }));
            mockRepository.Setup(x => x.Delete(It.IsAny<Product>()));
            mockRepository.Setup(x => x.SaveChangesAsync()).Returns(Task.FromResult(1));
            var controller = new ProductsController(mockRepository.Object);

            // Act
            var actionResult = await controller.DeleteProduct(Id);
            var contentResult = actionResult as OkNegotiatedContentResult<Product>;

            // Assert
            Assert.IsNotNull(contentResult);
            Assert.IsNotNull(contentResult.Content);
            Assert.AreEqual(Id, contentResult.Content.ID);
        }

        [TestMethod]
        public async Task DeleteProductNotFound()
        {
            // Arrange
            var mockRepository = new Mock<IRepository<Product>>();
            mockRepository.Setup(x => x.GetByIdAsync(Id)).Returns(Task.FromResult((Product)null));
            var controller = new ProductsController(mockRepository.Object);

            // Act
            var actionResult = await controller.DeleteProduct(Id);

            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(NotFoundResult));
        }
    }
}
