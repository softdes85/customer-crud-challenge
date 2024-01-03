using Customers.Management.Repository;
using System;
using Customers.Management.Repository.Entities;

namespace Customers.Management.Api.Seed
{
    public static class DbContextExtensions
    {
        public static void SeedData(this CustomerDBContext context)
        {
            if (!context.Customers.Any())
            {
                for (int i = 1; i <= 20; i++)
                {
                    context.Customers.Add(new Customer
                    {
                        Id = i,
                        FirstName = $"FirstName{i}",
                        LastName = $"LastName{i}",
                        Email = $"customer{i}@example.com",
                        CreatedDateTime = DateTime.UtcNow,
                        LastUpdatedDateTime = DateTime.UtcNow
                    });
                }

                context.SaveChanges();
            }
        }
    }
}
