using Customers.Management.Repository;
using Customers.Management.Repository.Entities;
using Customers.Management.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace Customers.Management.Repository.Repositories
{
    public class CustomersRepository: ICustomersRepository
    {
        private readonly CustomerDBContext _context;

        public CustomersRepository(CustomerDBContext context)
        {
            _context = context;
        }

        public async Task<List<Customer>> GetAllAsync()
        {
            return await _context.Customers.ToListAsync();
        }
        public async Task<IEnumerable<Customer>> GetAllAsync(int pageNumber, int pageSize)
        {
            return await _context.Customers
                                 .Skip((pageNumber - 1) * pageSize)
                                 .Take(pageSize)
                                 .ToListAsync();
        }
        public async Task<int> GetTotalCountAsync()
        {
            return await _context.Customers.CountAsync();
        }

        public async Task<Customer> GetByIdAsync(int id)
        {
            return await _context.Customers.FindAsync(id);
        }

        public async Task<Customer> AddAsync(Customer customer)
        {
            customer.CreatedDateTime = DateTime.UtcNow;
            customer.LastUpdatedDateTime = DateTime.UtcNow;
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();
            return customer;
        }

        public async Task UpdateAsync(Customer customer)
        {
            var existingCustomer = await _context.Customers.FindAsync(customer.Id);
            if (existingCustomer == null) return;

            existingCustomer.FirstName = customer.FirstName;
            existingCustomer.LastName = customer.LastName;
            existingCustomer.Email = customer.Email;
            existingCustomer.LastUpdatedDateTime = DateTime.UtcNow;

            _context.Entry(existingCustomer).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null) return;

            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();
        }
    }
}
