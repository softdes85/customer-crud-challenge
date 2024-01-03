using Customers.Management.Repository.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Customers.Management.Repository.Interfaces
{
    public interface ICustomersRepository
    {
        Task<List<Entities.Customer>> GetAllAsync();
        Task<IEnumerable<Customer>> GetAllAsync(int pageNumber, int pageSize);
        Task<int> GetTotalCountAsync();
        Task<Entities.Customer> GetByIdAsync(int id);
        Task<Entities.Customer> AddAsync(Entities.Customer customer);
        Task UpdateAsync(Entities.Customer customer);
        Task DeleteAsync(int id);
    }
}
