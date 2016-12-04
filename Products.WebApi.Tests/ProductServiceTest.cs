using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NLog;
using Products.WebApi.BL;
using Products.WebApi.DAL.Interfaces;
using Products.WebApi.DAL.Models;

namespace Products.WebApi.Tests
{
    /// <summary>
    /// Summary description for ProductServiceTest
    /// </summary>
    [TestClass]
    public class ProductServiceTest
    {
        private const int TestId = 10;
        private readonly Product _testProduct = new Product { Id = TestId, Name = "Xiaomi MI5", Price = 300 };

        [TestMethod]
        public void ProductService_GetProduct_ReturnsProduct()
        {
            Mock<IProductRepository> productRepositoryMock = new Mock<IProductRepository>();
            productRepositoryMock.Setup(x => x.GetByIdAsync(TestId)).Returns(Task.FromResult(_testProduct));

            Mock<ILogger> loggerMock = new Mock<ILogger>();
            loggerMock.Setup(x => x.Error(It.IsAny<Exception>(), It.IsAny<string>()));

            Mock<IUnitOfWork> unitOfWorkMock = new Mock<IUnitOfWork>();
            unitOfWorkMock.Setup(x => x.ProductRepository).Returns(productRepositoryMock.Object);

            using (var pkService = new ProductService(unitOfWorkMock.Object, loggerMock.Object))
            {
                var getProductResult = pkService.GetProduct(TestId).Result;

                Assert.IsTrue(getProductResult != null && getProductResult.Id == TestId);
            }
        }

        [TestMethod]
        public void ProductService_GetProduct_ReturnsNull()
        {
            Mock<IProductRepository> productRepositoryMock = new Mock<IProductRepository>();
            productRepositoryMock.Setup(x => x.GetByIdAsync(TestId)).Returns(Task.FromResult<Product>(null));

            Mock<ILogger> loggerMock = new Mock<ILogger>();
            loggerMock.Setup(x => x.Error(It.IsAny<Exception>(), It.IsAny<string>()));

            Mock<IUnitOfWork> unitOfWorkMock = new Mock<IUnitOfWork>();
            unitOfWorkMock.Setup(x => x.ProductRepository).Returns(productRepositoryMock.Object);

            using (var pkService = new ProductService(unitOfWorkMock.Object, loggerMock.Object))
            {
                var getProductResult = pkService.GetProduct(TestId).Result;

                Assert.IsTrue(getProductResult == null);
            }
        }

        [TestMethod]
        public void ProductService_PutProduct_ReturnsTrue()
        {
            Mock<IProductRepository> productRepositoryMock = new Mock<IProductRepository>();
            productRepositoryMock.Setup(x => x.Update(_testProduct));

            Mock<ILogger> loggerMock = new Mock<ILogger>();
            loggerMock.Setup(x => x.Error(It.IsAny<Exception>(), It.IsAny<string>()));

            Mock<IUnitOfWork> unitOfWorkMock = new Mock<IUnitOfWork>();
            unitOfWorkMock.Setup(x => x.ProductRepository).Returns(productRepositoryMock.Object);

            using (var pkService = new ProductService(unitOfWorkMock.Object, loggerMock.Object))
            {
                var putProductResult = pkService.PutProduct(10, _testProduct).Result;

                Assert.IsTrue(putProductResult);
            }
        }

        [TestMethod]
        public void ProductService_PutProduct_ConcurrentProductCase_ReturnsFalse()
        {
            var testProductExistsInDb = new Product { Id = TestId, Price = 400 };

            Mock<IProductRepository> productRepositoryMock = new Mock<IProductRepository>();
            productRepositoryMock.Setup(x => x.Update(_testProduct));
            productRepositoryMock.Setup(x => x.GetAsync(It.IsAny<Expression<Func<Product, bool>>>(), null, ""))
                .Returns(Task.FromResult(new List<Product> { testProductExistsInDb}));

            Mock<ILogger> loggerMock = new Mock<ILogger>();
            loggerMock.Setup(x => x.Error(It.IsAny<Exception>(), It.IsAny<string>()));

            Mock<IUnitOfWork> unitOfWorkMock = new Mock<IUnitOfWork>();
            unitOfWorkMock.Setup(x => x.ProductRepository).Returns(productRepositoryMock.Object);
            unitOfWorkMock.Setup(x => x.SaveChangesAsync()).Throws(new DbUpdateConcurrencyException("Test exception"));

            using (var pkService = new ProductService(unitOfWorkMock.Object, loggerMock.Object))
            {
                var putProductResult = pkService.PutProduct(10, _testProduct).Result;

                Assert.IsFalse(putProductResult);
            }
        }

        [TestMethod]
        public void ProductService_PutProduct_ConcurrentProductCase_ThrowsException()
        {
            Mock<IProductRepository> productRepositoryMock = new Mock<IProductRepository>();
            productRepositoryMock.Setup(x => x.Update(_testProduct));
            productRepositoryMock.Setup(x => x.GetAsync(It.IsAny<Expression<Func<Product, bool>>>(), null, ""))
                .Returns(Task.FromResult(new List<Product>(0)));

            Mock<ILogger> loggerMock = new Mock<ILogger>();
            loggerMock.Setup(x => x.Error(It.IsAny<Exception>(), It.IsAny<string>()));

            Mock<IUnitOfWork> unitOfWorkMock = new Mock<IUnitOfWork>();
            unitOfWorkMock.Setup(x => x.ProductRepository).Returns(productRepositoryMock.Object);
            unitOfWorkMock.Setup(x => x.SaveChangesAsync()).Throws(new DbUpdateConcurrencyException("Test exception"));

            using (var pkService = new ProductService(unitOfWorkMock.Object, loggerMock.Object))
            {
                bool exceptionThrown = false;
                try
                {
                    pkService.PutProduct(10, _testProduct).Wait();
                }
                catch (Exception ex)
                {
                    Assert.IsInstanceOfType(ex.InnerException, typeof(DbUpdateConcurrencyException));
                    exceptionThrown = true;
                }
                if (!exceptionThrown)
                    Assert.Fail();
            }
        }

        [TestMethod]
        public void ProductService_DeleteProduct_ReturnsProduct()
        {
            Mock<IProductRepository> productRepositoryMock = new Mock<IProductRepository>();
            productRepositoryMock.Setup(x => x.GetByIdAsync(TestId)).Returns(Task.FromResult(_testProduct));
            productRepositoryMock.Setup(x => x.Delete(_testProduct));

            Mock<ILogger> loggerMock = new Mock<ILogger>();
            loggerMock.Setup(x => x.Error(It.IsAny<Exception>(), It.IsAny<string>()));

            Mock<IUnitOfWork> unitOfWorkMock = new Mock<IUnitOfWork>();
            unitOfWorkMock.Setup(x => x.ProductRepository).Returns(productRepositoryMock.Object);
            unitOfWorkMock.Setup(x => x.SaveChangesAsync()).Returns(Task.CompletedTask);

            using (var pkService = new ProductService(unitOfWorkMock.Object, loggerMock.Object))
            {
                var deleteProductResult = pkService.DeleteProduct(TestId).Result;

                Assert.IsTrue(deleteProductResult != null && deleteProductResult.Id == TestId);
            }
        }

        [TestMethod]
        public void ProductService_DeleteProduct_ReturnsNull()
        {
            Mock<IProductRepository> productRepositoryMock = new Mock<IProductRepository>();
            productRepositoryMock.Setup(x => x.GetByIdAsync(TestId)).Returns(Task.FromResult<Product>(null));

            Mock<ILogger> loggerMock = new Mock<ILogger>();
            loggerMock.Setup(x => x.Error(It.IsAny<Exception>(), It.IsAny<string>()));

            Mock<IUnitOfWork> unitOfWorkMock = new Mock<IUnitOfWork>();
            unitOfWorkMock.Setup(x => x.ProductRepository).Returns(productRepositoryMock.Object);

            // let methods that not expected to be reached to throw exception
            productRepositoryMock.Setup(x => x.Delete(It.IsAny<Product>())).Throws<Exception>();
            unitOfWorkMock.Setup(x => x.SaveChangesAsync()).Throws<Exception>();

            using (var pkService = new ProductService(unitOfWorkMock.Object, loggerMock.Object))
            {
                var deleteProductResult = pkService.DeleteProduct(TestId).Result;

                Assert.IsNull(deleteProductResult);
            }
        }
    }
}
