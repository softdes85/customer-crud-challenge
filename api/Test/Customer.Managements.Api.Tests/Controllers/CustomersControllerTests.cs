using AutoMapper;
using Customers.Management.Api.Controllers;
using Customers.Management.Api.Dtos;
using Customers.Management.Repository.Entities;
using Customers.Management.Service.Interfaces;
using Customers.Management.Service.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Customers.Management.Api.Tests.Controllers
{
    [TestClass]
    public class CustomersControllerTests
    {
        private Mock<ICustomersService> mockService;
        private Mock<IMapper> mockMapper;
        private CustomersController controller;

        [TestInitialize]
        public void Setup()
        {
            // Arrange
            mockService = new Mock<ICustomersService>();
            mockMapper = new Mock<IMapper>();

            // Setup mapping from CustomerDto to Customer
            mockMapper.Setup(m => m.Map<Customer>(It.IsAny<CustomerDto>()))
                      .Returns((CustomerDto source) => new Customer
                      {
                          // Assign properties from source to destination
                          FirstName = source.FirstName,
                          LastName = source.LastName,
                          Email = source.Email
                      });

            controller = new CustomersController(mockService.Object, mockMapper.Object);
        }

        [TestMethod]
        public async Task GetAll_ReturnsAllCustomers()
        {
            // Arrange
            var mockService = new Mock<ICustomersService>();
            var mockMapper = new Mock<IMapper>();

            var testCustomers = GetTestCustomers(); // Get a list of test customers
            var totalRecords = testCustomers.Count;
            var pageNumber = 1;
            var pageSize = 10;

            mockService.Setup(s => s.GetAllCustomersAsync(pageNumber, pageSize))
                       .ReturnsAsync(testCustomers);
            mockService.Setup(s => s.GetTotalCountAsync())
                       .ReturnsAsync(totalRecords);

            mockMapper.Setup(m => m.Map<IEnumerable<CustomerDto>>(It.IsAny<IEnumerable<Customer>>()))
                      .Returns((IEnumerable<Customer> src) => src.Select(c => new CustomerDto { /* map properties */ }));

            var controller = new CustomersController(mockService.Object, mockMapper.Object);

            // Act
            var result = await controller.GetAll(pageNumber, pageSize);

            // Assert
            Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);

            var returnValue = okResult.Value as PaginatedResponse<CustomerDto>;
            Assert.IsNotNull(returnValue);
            Assert.AreEqual(totalRecords, returnValue.TotalCount);
            Assert.AreEqual(pageNumber, returnValue.CurrentPage);
            Assert.AreEqual(pageSize, returnValue.PageSize);
        }

        private List<Customer> GetTestCustomers()
        {
            return new List<Customer>
            {
                new Customer { FirstName = "Name1", LastName = "LName1", Email = "mail1@gmail.com" },
                new Customer { FirstName = "Name2", LastName = "LName2", Email = "mail2@gmail.com" },
            };
        }
    }
}
