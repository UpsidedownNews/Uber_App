using System.Collections.Generic;
using System.Threading.Tasks;
using CustomerServer.Models;

namespace CustomerServer.Data
{
    public interface IWebService
    {
        Task<Customer> ValidateUser(string mail, string password);
        Task<IList<Customer>> GetCustomerAsync();
        Task AddCustomerAsync(Customer customer);
        Task RemoveCustomerAsync(int id);
    }
}