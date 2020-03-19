using ASWA.Core.Entities;
using ASWA.Core.Interfaces;
using ASWA.Web.Interfaces;

namespace ASWA.Web.Services
{
    public class CustomerViewModelService: ICustomerViewModelService
    {
        private readonly IAsyncRepository<Customer> _customerRepository;

        public CustomerViewModelService(IAsyncRepository<Customer> customerRepository)
        {
            _customerRepository = customerRepository;
        }
    }
}