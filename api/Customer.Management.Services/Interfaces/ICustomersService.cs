using Customers.Management.Repository.Entities;

namespace Customers.Management.Service.Interfaces
{
    public interface ICustomersService
    {
        Task<IEnumerable<Customer>> GetAllCustomersAsync(int pageNumber, int pageSize);
        Task<IEnumerable<Repository.Entities.Customer>> GetAllCustomersAsync();
        Task<int> GetTotalCountAsync();
        Task<Repository.Entities.Customer> GetCustomerByIdAsync(int id);
        Task<Repository.Entities.Customer> CreateCustomerAsync(Customer customer);
        Task UpdateCustomerAsync(Customer customer);
        Task DeleteCustomerAsync(int id);
    }
}
