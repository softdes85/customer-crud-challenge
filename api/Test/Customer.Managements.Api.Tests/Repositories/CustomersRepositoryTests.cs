using Customers.Management.Repository.Entities;
using Customers.Management.Repository.Repositories;
using Customers.Management.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Customers.Management.Api.Tests.Repositories
{
    [TestClass]
    public class CustomersRepositoryTests
    {
        private CustomersRepository repository;
        private CustomerDBContext context;

        [TestInitialize]
        public void Setup()
        {
            // Setup in-memory database
            var options = new DbContextOptionsBuilder<CustomerDBContext>()
                              .UseInMemoryDatabase(databaseName: "TestDatabase")
                              .Options;
            context = new CustomerDBContext(options);

            // Seed the database with test data
            context.Customers.Add(new Customer {  });
            context.Customers.Add(new Customer {  });
            context.SaveChanges();

            // Create repository instance
            repository = new CustomersRepository(context);
        }

        [TestMethod]
        public async Task GetAllAsync_ReturnsAllCustomers()
        {
            // Act
            var customers = await repository.GetAllAsync();

            // Assert
            Assert.AreEqual(2, customers.Count); // Assuming 2 customers were added in the setup
        }

        [TestCleanup]
        public void Cleanup()
        {
            context.Database.EnsureDeleted();
            context.Dispose();
        }
    }
}
