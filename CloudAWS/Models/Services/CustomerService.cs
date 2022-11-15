using Omu.ValueInjecter;

namespace CloudAWS.Models.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomerService(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<bool> CreateAsync(Customer customer)
        {
            var existingUser = await _customerRepository.GetAsync(customer.Id);
            if (existingUser is not null)
            {
                return false;
            }

            var customerDto = new CustomerDto();
            customerDto.InjectFrom(customer);
            customerDto.Pk = customer.Id.ToString();
            customerDto.Sk = customer.Id.ToString();
            customerDto.Id = customer.Id.ToString();
            return await _customerRepository.CreateAsync(customerDto);
        }

        public async Task<Customer> GetAsync(Guid id)
        {
            var customerDto = await _customerRepository.GetAsync(id);
            var customer = new Customer();
            customer.InjectFrom(customerDto);
            customer.Id = Guid.Parse(customerDto.Id);
            return customer;
        }

        public async Task<IEnumerable<Customer>> GetAllAsync()
        {
            var customerDtos = await _customerRepository.GetAllAsync();
            return customerDtos.Select(x => new Customer() { 
                Id=Guid.Parse(x.Id),
                Email=x.Email,
                DateOfBirth=x.DateOfBirth,
                GitHubUsername=x.GitHubUsername,
                FullName=x.FullName
        });
        }

        public async Task<bool> UpdateAsync(Customer customer)
        {
            var customerDto = new CustomerDto();
            customerDto.InjectFrom(customer);
            customerDto.Pk = customer.Id.ToString();
            customerDto.Sk = customer.Id.ToString();
            customerDto.Id = customer.Id.ToString();
            return await _customerRepository.UpdateAsync(customerDto);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            return await _customerRepository.DeleteAsync(id);
        }

    }
}
