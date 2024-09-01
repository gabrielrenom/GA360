using GA360.DAL.Entities.Entities;
using GA360.DAL.Infrastructure.Interfaces;
using GA360.Domain.Core.Interfaces;

namespace GA360.Domain.Core.Services;

public class CustomerService : ICustomerService
{
    private readonly ICustomerRepository _customerRepository;

    public CustomerService(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public Customer GetCustomerById(int id)
    {
        return _customerRepository.Get(id);
    }

    public Customer GetCustomerByEmail(string email)
    {
        return _customerRepository.GetCustomerByEmail(email);
    }

    public IEnumerable<Customer> GetCustomersByCountry(int countryId)
    {
        return _customerRepository.GetCustomersByCountry(countryId);
    }

    public async Task<Customer> AddCustomer(Customer customer)
    {
        _customerRepository.Add(customer);
        await _customerRepository.SaveChangesAsync();
        return customer;
    }

    public void UpdateCustomer(Customer customer)
    {
        _customerRepository.Update(customer);
        _customerRepository.SaveChanges();
    }

    public void DeleteCustomer(int id)
    {
        var customer = _customerRepository.Get(id);
        if (customer != null)
        {
            _customerRepository.Delete(customer);
            _customerRepository.SaveChanges();
        }
    }
}