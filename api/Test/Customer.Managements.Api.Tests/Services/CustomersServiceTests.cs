using Customers.Management.Repository.Entities;
using Customers.Management.Repository.Interfaces;
using Customers.Management.Service.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Customers.Management.Api.Tests.Services
{
    [TestClass]
    public class CustomersServiceTests
    {
        private Mock<ICustomersRepository> mockRepository;
        private CustomersService service;

        [TestInitialize]
        public void Setup()
        {
            mockRepository = new Mock<ICustomersRepository>();

            // Create an instance of the service with the mocked repository
            service = new CustomersService(mockRepository.Object);

            // Mock the repository's GetAllAsync method to return a list of test customers
            var testCustomers = GetTestCustomers();
            mockRepository.Setup(repo => repo.GetAllAsync(It.IsAny<int>(), It.IsAny<int>()))
                          .ReturnsAsync(testCustomers);
        }

        [TestMethod]
        public async Task GetAllCustomersAsync_ReturnsAllCustomers()
        {
            // Arrange
            int pageNumber = 1;
            int pageSize = 10;

            // Act
            var result = await service.GetAllCustomersAsync(pageNumber, pageSize);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count()); // Assuming GetTestCustomers returns 2 customers
            mockRepository.Verify(r => r.GetAllAsync(pageNumber, pageSize), Times.Once);
        }

        private IEnumerable<Customer> GetTestCustomers()
        {
            // Return a list of test customers
            return new List<Customer>
        {
            new Customer { /* ... initialize properties ... */ },
            new Customer { /* ... initialize properties ... */ }
        };
        }
    }
}
