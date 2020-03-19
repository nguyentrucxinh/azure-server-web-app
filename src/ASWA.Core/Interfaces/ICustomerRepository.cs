using ASWA.Core.Entities;
using System.Threading.Tasks;

namespace ASWA.Core.Interfaces
{
    public interface ICustomerRepository : IAsyncRepository<Customer>
    {
        Task<Customer> GetByEmailAsync(string email);
    }
}